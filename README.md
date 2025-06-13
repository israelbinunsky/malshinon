# class program
### Main()  
### menue()
reporterIdentification (class report)  
options:  
addReport (class report)  
getAllAlerts (class stats)  
getAllAgents (class stats)  
# class report
### reporterIdentification()
isManExist (class peopleDal)  
if false: SetNewMan (class peopleDal)  
else: getPersonType (class peopleDal) if he "target": updateManType (class updates) to "both"  
### addReport(string[] reporterNames)
getTargetName (class local)  
isManExist. if false: SetNewMan, else: getPersonType. if he "reporter": updateManType to "both"  
then insert report to table with reporter and target id's  
addCount (class updates): reporter - num_reports; target - num_mentions  
reporterToAgent (class updates)   
targetToThreat (class updates)  
# class peopleDal
###  isManExist(string[] names)
### SetNewMan(string[] names, string type)
secret code: ganarateCode (class local)
### getPersonId(string[] names)
### getPersonName(int id)
### getPersonType(int id)
### getNumReports(int id)
### getNumMentions(int id)
# class local
### ganarateCode(int len)
### getTargetName(string txt)
find Capitalized name from text report  
# class updates
### addCount(string type, int id)
### updateManType(int id, string type)
### reporterToAgent(int id)
getPersonType (peopleDal)  
if != "potential_agent": getNumReports (peopleDal)     
if > 9: calculateAverageLen (updates)    
if > 15: updateManType (updates)  
### targetToThreat(int id)
getPersonType  
if != "potential threat": getNumMentions (peopleDal)  
if > 9:   
or isIn15Min (updates)    
if true:  
updateManType  
addAlert (alerts)  
### getReportsLens(int id)
return list of nums: len of each report  
### calculateAverageLen(int id)
getReportsLens (updates)  
### get3Datetimes(int targetId)
### isIn15Min(int targetId)
get3Datetimes (updates)
is all 3 in 15 minutes
# class alerts
### addAlert(int targetId)
add target id to alerts table, datetime auto incrument  
# class stats
### getAllAlerts()
### getAllAgents()
