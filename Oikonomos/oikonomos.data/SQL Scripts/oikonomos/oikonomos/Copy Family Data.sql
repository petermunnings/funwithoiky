insert into Family (FamilyName, AddressId, ChurchId)
(
select f.Surname, f.AddressId, f.ChurchId
from (
select p.Surname, p.AddressId, p.ChurchId, ROW_NUMBER() over (PARTITION BY p.Surname Order By p.Created Desc) rn
from Person p 
left join family f
on p.surname = f.familyname
where f.familyid is null) f
where rn=1
)

update p set p.FamilyId = f.FamilyId
from Person p
inner join Family f
on p.Surname = f.FamilyName

update f set f.HomePhone = pc.Contact
from Family f
inner join Person p
on f.FamilyId = p.FamilyId
inner join PersonContact pc
on p.PersonId = pc.PersonId
where pc.ContactTypeId = 1


