MERGE (ana:Hero:Support {Name:'Ana', Health: 200})
MERGE (zen:Hero:Support {Name:'Zenyatta'})
MERGE (road:Hero:Tank {Name:'Roadhog', Health: 600})
MERGE (winst:Hero:Tank {Name:'Winston', Health: 500})
MERGE (junk:Hero:DPS {Name:'Junkrat', Health: 200})

MATCH (a:Hero),(b:Hero)
WHERE a.Name = 'Ana' AND b.Name = 'Zenyatta'
CREATE (a)-[r:CounterSkill|Control {Value:0.40}]->(b)



MERGE (a {Name:'Ana'})->[:CounterSkill|Control {Value:0.40}]->(zen:Hero {Name:'Zenyatta'})
MERGE (ana)-[:CounterSkill|Control {Value:0.25}]->(road)
MERGE (ana)<-[:CouldHaunt|Control {Value:0.15}]-(winst)