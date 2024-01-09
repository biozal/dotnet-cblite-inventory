FROM couchbase/server:latest AS stage_base
# FROM couchbase:latest AS stage_base 
COPY init-cbserver.sh /opt/couchbase/init/
COPY ../data-gen/beer_items.json /opt/couchbase/init/
COPY ../data-gen/customers.json /opt/couchbase/init/
COPY ../data-gen/districts_atlanta.json /opt/couchbase/init/
COPY ../data-gen/districts_chicago.json /opt/couchbase/init/
COPY ../data-gen/districts_denver.json /opt/couchbase/init/
COPY ../data-gen/districts_detroit.json /opt/couchbase/init/
COPY ../data-gen/districts_houston.json /opt/couchbase/init/
COPY ../data-gen/districts_jacksonville.json /opt/couchbase/init/
COPY ../data-gen/districts_kanascity.json /opt/couchbase/init/
COPY ../data-gen/districts_nashville.json /opt/couchbase/init/
COPY ../data-gen/districts_newyork.json /opt/couchbase/init/
COPY ../data-gen/districts_philadelphia.json /opt/couchbase/init/
COPY ../data-gen/districts_santaclara.json /opt/couchbase/init/
COPY ../data-gen/districts_seattle.json /opt/couchbase/init/
COPY ../data-gen/offices.json /opt/couchbase/init/
COPY ../data-gen/orders.json /opt/couchbase/init/
COPY ../data-gen/projects.json /opt/couchbase/init/
COPY ../data-gen/warehouses.json /opt/couchbase/init/
COPY ../data-gen/userProfiles.json /opt/couchbase/init/
