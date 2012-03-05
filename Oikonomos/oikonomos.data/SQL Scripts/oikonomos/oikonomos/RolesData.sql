exec sp_executesql N'
insert into [role] (RoleId, [Name])
values (1, 'Church Administrator')

insert into [role] (RoleId, [Name])
values (2, 'Group Administrator')


insert into [role] (RoleId, [Name])
values (3, 'Church Member')
'