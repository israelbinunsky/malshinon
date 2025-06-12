# class program
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
# class peopleDal
###  isManExist(string[] names)
### SetNewMan(string[] names, string type)
### getPersonId(string[] names)
### getPersonName(int id)
### getPersonType(int id)
### getNumReports(int id)
### getNumMentions(int id)
# class local
### ganarateCode(int len)
### getTargetName(string txt)
# class updates
### addCount(string type, int id)
### updateManType(int id, string type)
### reporterToAgent(int id)
### targetToThreat(int id)
### getReportsLens(int id)
### calculateAverageLen(int id)
### getDatetimes(int targetId)
### isIn15Min(int targetId)
# class alerts
### addAlert(int targetId)
# class stats
### getAllAlerts()
### getAllAgents()
