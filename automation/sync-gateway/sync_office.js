function (doc, oldDoc, meta) {

	const systemAdmin = "sysAdmin";

	//only system admins can delete offices
	if (doc._deleted){
		requireRole(systemAdmin);	
		return;
	}
	if (!doc.officeId) {
		throw({forbidden : "officeId is required"});
	}

	//only system admins can create or update offices
	requireRole(systemAdmin);

	//allow all users to sync all offices
	channel("*");
}