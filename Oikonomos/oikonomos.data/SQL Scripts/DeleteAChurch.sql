delete from pr
from PersonRelationship pr
join PersonChurch pc
on pr.PersonId = pc.PersonId
where pc.ChurchId = 11

delete from pe
from PersonEvent pe
join PersonChurch pc
on pe.PersonId = pc.PersonId
where pc.ChurchId = 11

delete from pg
from PersonGroup pg
join PersonChurch pc
on pg.PersonId = pc.PersonId
where pc.ChurchId = 11

delete from [Group]
where ChurchId = 11

delete from [Event]
where ChurchId = 11

delete from OldEvent
where ChurchId = 11

delete from pof
from PersonOptionalField pof
join PersonChurch pc
on pof.PersonId = pc.PersonId
where pc.ChurchId = 11

delete from c
from Comment c
join PersonChurch pc
on c.AboutPersonId = pc.PersonId
where pc.ChurchId = 11

delete from c
from Comment c
join PersonChurch pc
on c.MadeByPersonId = pc.PersonId
where pc.ChurchId = 11

delete from GroupClassification
where ChurchId = 11

delete from PersonChurch
where ChurchId = 11

delete from P
from Person p
left join PersonChurch pc
on p.PersonId = pc.PersonId
where pc.ChurchId IS NULL 
 
delete from Site where ChurchId = 11

delete from f
from Family f
left join Person p
on f.FamilyId = p.FamilyId
where p.PersonId is null

delete from a
from Address a
LEFT join family f
on a.AddressId = f.AddressId
left join Church c
on a.AddressId = c.AddressId
left join [Group] g
on a.AddressId = g.AddressId
left join [Site] s
on a.AddressId = s.AddressId
where f.FamilyId is null 
  and c.ChurchId is null 
  and g.GroupId is null
  and s.SiteId is null
