#!/bin/bash

# ****************************************
# used to start couchbase server - can't get around this as docker compose only allows you to start one command - so we have to start couchbase like the standard couchbase Dockerfile would 
# https://github.com/couchbase/docker/blob/master/enterprise/couchbase-server/7.2.0/Dockerfile#L88
# ****************************************

/entrypoint.sh couchbase-server & 


# ****************************************
# track if setup is complete so we don't try to setup again
# ****************************************
FILE=/opt/couchbase/init/setupComplete.txt

if ! [ -f "$FILE" ]; then
  # ****************************************
  # used to automatically create the cluster based on environment variables
  # https://docs.couchbase.com/server/current/cli/cbcli/couchbase-cli-cluster-init.html
  # ****************************************

  echo $COUCHBASE_ADMINISTRATOR_USERNAME ":"  $COUCHBASE_ADMINISTRATOR_PASSWORD  

  sleep 10s 
  /opt/couchbase/bin/couchbase-cli cluster-init -c 127.0.0.1 \
  --cluster-username $COUCHBASE_ADMINISTRATOR_USERNAME \
  --cluster-password $COUCHBASE_ADMINISTRATOR_PASSWORD \
  --services data,index,query,eventing \
  --cluster-ramsize $COUCHBASE_RAM_SIZE \
  --cluster-index-ramsize $COUCHBASE_INDEX_RAM_SIZE \
  --cluster-eventing-ramsize $COUCHBASE_EVENTING_RAM_SIZE \
  --index-storage-setting default

  sleep 2s 

  # ****************************************
  # used to auto create the bucket based on environment variables
  # https://docs.couchbase.com/server/current/cli/cbcli/couchbase-cli-bucket-create.html
  # ****************************************

  /opt/couchbase/bin/couchbase-cli bucket-create -c localhost:8091 \
  --username $COUCHBASE_ADMINISTRATOR_USERNAME \
  --password $COUCHBASE_ADMINISTRATOR_PASSWORD \
  --bucket $COUCHBASE_BUCKET \
  --bucket-ramsize $COUCHBASE_BUCKET_RAMSIZE \
  --bucket-type couchbase 

  sleep 2s 

  # ****************************************
  # used to auto create the sync gateway user based on environment variables  
  # https://docs.couchbase.com/server/current/cli/cbcli/couchbase-cli-user-manage.html#examples
  # ****************************************

  /opt/couchbase/bin/couchbase-cli user-manage \
  --cluster http://127.0.0.1 \
  --username $COUCHBASE_ADMINISTRATOR_USERNAME \
  --password $COUCHBASE_ADMINISTRATOR_PASSWORD \
  --set \
  --rbac-username $COUCHBASE_RBAC_USERNAME \
  --rbac-password $COUCHBASE_RBAC_PASSWORD \
  --roles mobile_sync_gateway[*] \
  --auth-domain local

  sleep 2s 

  # ****************************************
  # used to auto create the scopes
  # https://docs.couchbase.com/server/current/rest-api/creating-a-scope.html
  # ****************************************
  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=retail'

  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=audit'

  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=workorders'

  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=personnel'

  # ****************************************
  # used to auto create the collections 
  # https://docs.couchbase.com/server/current/rest-api/creating-a-collection.html
  # ****************************************

  # ****************************************
  # retail collections
  # ****************************************
  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes/retail/collections \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=customers'

  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes/retail/collections \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=orders'

  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes/retail/collections \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=warehouses'

  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes/retail/collections \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=districts'

  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes/retail/collections \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=items'

  # ****************************************
  # work orders collections
  # ****************************************
  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes/workorders/collections \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=projects'

  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes/workorders/collections \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=workItems'

  # ****************************************
  # audit collections
  # ****************************************
  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes/audit/collections \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=internalInventory'

  # ****************************************
  # personnel collections
  # ****************************************
  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes/personnel/collections \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=offices'

  /opt/couchbase/bin/curl -v http://localhost:8091//pools/default/buckets/$COUCHBASE_BUCKET/scopes/personnel/collections \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'name=userProfiles'

  # ****************************************
  # create indexes using the QUERY REST API  
  # ****************************************
  /opt/couchbase/bin/curl -v http://localhost:8093/query/service \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'statement=CREATE INDEX customerIdOrderDateIdx on ' + $COUCHBASE_BUCKET + '.retail.orders(customerId,orderDate)'

  /opt/couchbase/bin/curl -v http://localhost:8093/query/service \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'statement=CREATE INDEX customerCityStateIdx on ' + $COUCHBASE_BUCKET + '.retail.customers(city,state)'

  /opt/couchbase/bin/curl -v http://localhost:8093/query/service \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'statement=CREATE INDEX warehouseCityStateIdx on ' + $COUCHBASE_BUCKET + '.retail.warehouses(city,state)'

  /opt/couchbase/bin/curl -v http://localhost:8093/query/service \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'statement=CREATE INDEX districtsCityStateIdx on ' + $COUCHBASE_BUCKET + '.retail.districts(city,state)'

  /opt/couchbase/bin/curl -v http://localhost:8093/query/service \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'statement=CREATE INDEX itemsNamePriceStyleIdx on ' + $COUCHBASE_BUCKET + '.retail.items(name,price,style)'

  /opt/couchbase/bin/curl -v http://localhost:8093/query/service \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'statement=CREATE INDEX itemsStyleIdx on ' + $COUCHBASE_BUCKET + '.retail.items(style)'
      
  /opt/couchbase/bin/curl -v http://localhost:8093/query/service \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'statement=CREATE INDEX projectsTypeTeamIdx on ' + $COUCHBASE_BUCKET + '.workorders.projects(teamId, projectType)'

  /opt/couchbase/bin/curl -v http://localhost:8093/query/service \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'statement=CREATE INDEX projectsTeamIdx on ' + $COUCHBASE_BUCKET + '.personnel.offices(officeId)'

  /opt/couchbase/bin/curl -v http://localhost:8093/query/service \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'statement=CREATE INDEX projectsTeamIdx on ' + $COUCHBASE_BUCKET + '.personnel.userProfiles(userProfileId)'

  /opt/couchbase/bin/curl -v http://localhost:8093/query/service \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME:$COUCHBASE_ADMINISTRATOR_PASSWORD \
  -d 'statement=CREATE INDEX projectsTeamIdx on ' + $COUCHBASE_BUCKET + '.personnel.userProfiles(teams)'
  sleep 2s

  # import sample data into the bucket
  # https://docs.couchbase.com/server/current/tools/cbimport-json.html

  # load warehouses
  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/warehouses.json" \
  -f list \
  -g %warehouseId% \
  --scope-collection-exp retail.warehouses \
  -t 4 \
  
  # load districts
  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_atlanta.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_chicago.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_denver.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_detroit.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_houston.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_jacksonville.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_kansascity.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_nashville.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_newyork.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_philadelphia.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_santaclara.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/districts_seattle.json" \
  -f list \
  -g %districtId% \
  --scope-collection-exp retail.districts \
  -t 4 \

  # load items
  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/beer_items.json" \
  -f list \
  -g %itemId% \
  --scope-collection-exp retail.items \
  -t 4 \

  # load customers and orders 
  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/customers.json" \
  -f list \
  -g %customerId% \
  --scope-collection-exp retail.customers \
  -t 4 \

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/orders.json" \
  -f list \
  -g %orderId% \
  --scope-collection-exp retail.orders \
  -t 4 \

  # work orders
   /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/projects.json" \
  -f list \
  -g %orderId% \
  --scope-collection-exp workorders.projects \
  -t 4 \ 

  # personnel 
  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/offices.json" \
  -f list \
  -g %orderId% \
  --scope-collection-exp personnel.offices \
  -t 4 \ 

  /opt/couchbase/bin/cbimport json \
  -c http://localhost:8091 \
  -u $COUCHBASE_ADMINISTRATOR_USERNAME \
  -p $COUCHBASE_ADMINISTRATOR_PASSWORD \
  -b $COUCHBASE_BUCKET \
  -d "file:///opt/couchbase/init/userProfiles.json" \
  -f list \
  -g %orderId% \
  --scope-collection-exp personnel.userProfiles \
  -t 4 \ 

  # create file so we know that the cluster is setup and don't run the setup again 
  touch $FILE

  # remove sample data files to save on space on the docker image
  rm -rf /opt/couchbase/init/*.json
fi 
  # docker compose will stop the container from running unless we do this
  # known issue and workaround
  tail -f /dev/null

