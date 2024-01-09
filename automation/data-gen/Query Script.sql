--First look; deliveries by warehouse.
--Note unnesting of embedded array of objects.
select sum(ol.quantity) quantity, ol.warehouse
from orders o, o.orderLines ol
where o.orderDate >= '2022-07-01'
  and o.orderDate < '2022-08-01'
group by ol.warehouse
order by sum(ol.quantity) desc;

--Summary by demographic group.
--Note unnest, joins, "let" statement and rollup.
select sum(ol.quantity) quantity, buyingGroup, i.style
from orders o, o.orderLines ol
join customers c on o.customerId = c.customerId
join items i on ol.itemId = i.itemId
let buyingGroup = concat(c.personal.sex,'-',to_string(ageGroup(c.personal.birthdate)))
where o.orderDate >= '2022-07-01'
  and o.orderDate < '2022-08-01'
group by rollup (buyingGroup, i.style)
order by buyingGroup, i.style;

--How do we spend our advertising dollars?  Which demos/styles to target?
select sum(ol.quantity) quantity, ageGroup(c.personal.birthdate) ageGroup, c.personal.sex, i.style
from orders o, o.orderLines ol
join customers c on o.customerId = c.customerId
join items i on ol.itemId = i.itemId
where o.orderDate >= '2022-07-01'
  and o.orderDate < '2022-08-01'
group by ageGroup(c.personal.birthdate), c.personal.sex, i.style
order by sum(ol.quantity) desc;

--How about regional differences?
select sum(ol.quantity) quantity, c.state, i.style
from orders o, o.orderLines ol
join customers c on o.customerId = c.customerId
join items i on ol.itemId = i.itemId
where o.orderDate >= '2022-07-01'
  and o.orderDate < '2022-08-01'
group by i.style, c.state
order by sum(ol.quantity) desc;

--Distribution of quantity sold by ad regions.
--Note window function, subquery, ad hoc field mapping.
select a.*, round(ratio_to_report(quantity) over (partition by adRegion order by quantity DESC)*100,0) pct from
(
select sum(ol.quantity) quantity, adRegion, i.style
from orders o, o.orderLines ol
join customers c on o.customerId = c.customerId
join items i on ol.itemId = i.itemId
let adRegion =
case
  when c.state in ['ID','MT','OR','WA'] then 'Birkenstockers'
  when c.state in ['AK','CA','HI'] then 'CAHIAK'
  when c.state in ['AL','FL','LA','MS'] then 'Gulf States'
  when c.state in ['IL','MN','ND','SD','WI'] then 'Ice Fisherman'
  when c.state in ['DC','DE','MD','PA','VA'] then 'Mid-Atlantic'
  when c.state in ['AZ','CO','NM','NV','UT','WY'] then 'Mountain SW'
  when c.state in ['AR','GA','KY','NC','SC','TN'] then 'NASCARland'
  when c.state in ['CT','MA','ME','NH','RI','VT'] then 'New Englanders'
  when c.state in ['NJ','NY'] then 'NYNJ'
  when c.state in ['IN','MI','OH','WV'] then 'Rust Belt'
  when c.state in ['IA','KS','MO','NE'] then 'Salt of the earth'
  when c.state in ['OK','TX'] then 'TXOK'
  else ''
end
where o.orderDate >= '2022-07-01'
  and o.orderDate < '2022-08-01'
group by adRegion, i.style
) a
order by adRegion;


--Let's see what our average delivery times looked liked last month
select ol.warehouse, round(avg(date_diff_str(ol.deliveryDate, o.orderDate, 'day')),1) avgDeliveryDays
from orders o, o.orderLines ol
where o.orderDate >= '2022-07-01'
  and o.orderDate < '2022-08-01'
group by ol.warehouse
order by round(avg(date_diff_str(ol.deliveryDate, o.orderDate, 'day')),1) desc;

--Kansas city is averaging over four days.  Let's drill down.
select c.city, c.state, c.zip, round(avg(date_diff_str(ol.deliveryDate, o.orderDate, 'day')),1) avgDeliveryDays, ol.warehouse
from orders o, o.orderLines ol
join customers c on o.customerId = c.customerId
where o.orderDate >= '2022-07-01'
  and o.orderDate < '2022-08-01'
  and ol.warehouse = 'Kansas City Warehouse'
group by ol.warehouse, c.zip, c.city, c.state
order by avg(date_diff_str(ol.deliveryDate, o.orderDate, 'day')) desc;

--Now see if there are closer warehouses.
--Note non-equi join, UDF, and on-the-fly boolean in where clause.
select o.orderId, ol.warehouse, c.city, c.state, ow.name otherWarehouse,
       deliveryDistance,
       otherWarehouseDeliveryDistance
from orders o, o.orderLines ol
join customers c on o.customerId = c.customerId
join warehouses w on ol.warehouseId = w.warehouseId
join warehouses ow on ol.warehouseId <> ow.warehouseId
let deliveryDistance = distance(c.personal.loc[1], c.personal.loc[0], w.latitude, w.longitude),
    otherWarehouseDeliveryDistance = distance(c.personal.loc[1], c.personal.loc[0], ow.latitude, ow.longitude),
    closerWarehouseFound = deliveryDistance > otherWarehouseDeliveryDistance
where o.orderDate >= '2022-07-01'
  and o.orderDate < '2022-08-01'
  and ol.warehouse = 'Kansas City Warehouse'
  and ol.lineNumber = 1
  and closerWarehouseFound = true;

--Query/FTS examples (from query console):
--Sedgwick County has decide to go dry.  We wonder how this will affect our business.
--We see that Witchita is the major city there.  Let see how many customers are within 50 miles.
select count(meta(c).id) customerCount
from gartner.retail.customers as c
where search(c,
  {
  "query": {
    "location": {
      "lon": -97.3301,
      "lat": 37.6872
      },
    "distance": "50mi",
    "field": "personal.loc"
    }
  }
);

--Legal says that is not good enough.  We need to know exactly which customers
--are in the actual county borders.  We can do this for any polygon given its vertices.
select count(meta(c).id) customerCount
from gartner.retail.customers AS c
where search(c,
  {
  "query":
    {
    "field": "personal.loc",
    "polygon_points": [
        "37.476,-97.151",
        "37.475,-97.807",
        "37.733,-97.807",
        "37.735,-97.698",
        "37.825,-97.699",
        "37.825,-97.702",
        "37.912,-97.702",
        "37.913,-97.153",
        "37.476,-97.151"
      ]
    }
  }
);

--Now we need to see exactly what this is going to do to our monthly revenue.
--Note the CTE as well as the combination of FTS and Query services
with dryCustomers as
(
select raw meta(c).id
from gartner.retail.customers AS c
where search(c,
  {
  "query":
    {
    "field": "personal.loc",
    "polygon_points": [
        "37.476,-97.151",
        "37.475,-97.807",
        "37.733,-97.807",
        "37.735,-97.698",
        "37.825,-97.699",
        "37.825,-97.702",
        "37.912,-97.702",
        "37.913,-97.153",
        "37.476,-97.151"
      ]
    }
  }
)
)
select sum(ol.quantity) quantity, round(sum(ol.amount)) amount, substr(o.orderDate,0,7) ym from gartner.retail.orders o
unnest o.orderLines ol
where o.customerId in dryCustomers
group by substr(o.orderDate,0,7)
order by substr(o.orderDate,0,7) desc;

--If need, other plain full text search examples:
-- Customer Service gets a call from someone named Wallace in TX.
select c.firstName, c.lastName, c.city
from gartner.retail.customers c
where search(c.lastName, "Wallace")
  and c.state = 'TX';

--Plain FTS text search - Inside sales gets a call from someone who likes bay leaf ale.
select i.name, i.style
from gartner.retail.items as i
where search(i,
  {
  "query":
    {
    "field": "name",
    "match": "bay lea"
    }
  }
  )
 and i.style = "Ale";
