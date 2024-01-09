function sync (doc, oldDoc, meta) {

	const systemAdmin = "sysAdmin";

	//only system admins can delete user profiles
	if (isDelete()){
		requireRole(systemAdmin);	
		return;
	}

	if (!doc.email) {
		throw({forbidden : "email is required"});
	} else {

		//first validate the documentId field is in proper format and that someone isn't trying to change the documentId field
		
		const docId = "profile::" + doc.email;
		if (doc.userProfileId != docId && doc._id != docId ) {
			throw({forbidden : "documentId be in format profile::<email>"});
		}
		if (isEmailNotChanged(oldDoc, doc)){
			throw({forbidden : "Can't change email address/documentId."});
		}

		//require the assigned user to the doc to be the same user if updating
		requireUser(doc.email);

		//allow all users to see all user profiles - they just can't create/edit them
		channel("*");
	}

	function isEmailNotChanged(oldDoc, doc){
		if(oldDoc != null){ 
			if ((oldDoc.userProfileId != docId.userProfileId) || 
			(oldDoc.email != doc.email)){
					return true;
			}
		}
		return false;
	}

	function isDelete(){
		return (doc._deleted == true);
	}
}