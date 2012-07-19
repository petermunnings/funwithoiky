
INSERT INTO Permission (PermissionId, Name, Description, Category, IsVisible)
VALUES (53, 'SendVisitorWelcomeLetter', 'SendVisitorWelcomeLetter', 'Basic', 1)

INSERT INTO PermissionRole (PermissionId, RoleId)
select 53, RoleId from [Role] where Name = 'Visitor'

CREATE TABLE [dbo].[EmailTemplate](
	[EmailTemplateId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED 
(
	[EmailTemplateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

INSERT INTO EmailTemplate (EmailTemplateId, Name) 
VALUES (1, 'Welcome Visitors')

INSERT INTO EmailTemplate (EmailTemplateId, Name) 
VALUES (2, 'Welcome Members')

INSERT INTO EmailTemplate (EmailTemplateId, Name) 
VALUES (3, 'Notify Group Leader')

ALTER TABLE dbo.ChurchEmailTemplate ADD CONSTRAINT
	FK_ChurchEmailTemplate_Church FOREIGN KEY
	(
	ChurchId
	) REFERENCES dbo.Church
	(
	ChurchId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.ChurchEmailTemplate ADD CONSTRAINT
	FK_ChurchEmailTemplate_EmailTemplate FOREIGN KEY
	(
	EmailTemplateId
	) REFERENCES dbo.EmailTemplate
	(
	EmailTemplateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 

insert into ChurchEmailTemplate (ChurchId, EmailTemplateId, Template)
select churchid, 2, '<html>
<head>
    <title>Welcome to ##SystemName## </title>
    <style type="text/css">
        @font-face
        {
            font-family:Calibri
        }
        
        a:link, span.MsoHyperlink
        {
            color:#2F4419;
            text-decoration:none none;
        }
        
        p
        {
            margin-top:0cm;
            margin-right:0cm;
            margin-bottom:0cm;
            margin-left:0pt;
            margin-bottom:.0001pt;
            text-align:justify;
            font-size:14.0pt;
            font-family:"Calibri";
            color:#043144;
            }
        li
        {
            font-size:14.0pt;
            font-family:"Calibri";
            color:#043144;
            text-align:left;
            }
    </style>
</head>
<body style="background-color: #FFFFFF">
    <div>
        <div align="left">
         
   <div style="display:table">
                <div style="display:table-row">
                    <div style="display:table-cell; width:12.75pt;padding:0cm 0cm 0cm 0cm">
                    </div>
                </div>
                <div style="display:table-row">
                    <div style="display:table-cell; width:12.75pt;padding:0cm 0cm 0cm 0cm">
                    </div>                
                    <div style="display:table-cell; width:537.75pt;padding:0cm 0cm 0cm 0cm">
                        <p>
                            <span style="font-size:14.0pt">Hi ##Firstname## ##Surname##</span>&nbsp;
                        </p>
                        <p>&nbsp;</p>
                        <p>This is to let you know that you now have access to ##SystemName##.  This is a fun new way of keeping in touch with people in your church.  You''ll be able to <ul><li>update your details</li><li>lookup other people''s details</li><li>see what events/birthdays/anniversaries are about to happen</li></ul>
                        </p>
                        <p>##SystemName## is completely private and safe and your details are not shared with anyone outside the church.  Even if you log in using facebook - nothing is sent to facebook from ##SystemName##.  It simply uses facebook to check that you are who you say you are and it fetches your profile picture so people who are good with faces and not names can remember you.
                        <p>&nbsp;</p>
                        <p>In order to access it, go to <a href="https://www.oikonomos.co.za/Home/Login/##PublicId##">www.oikonomos.co.za</a> and do one of the following:</p>
                        <ul><li>If you have a facebook account you can click on the <b>Login using facebook button</b>.  This will then use your facebook username and password to connect you to the system.  It saves you remembering another password, and gives you additional functionality on ##SystemName##</li>
                        <li>If not, you can login with the following credentials</li></ul>
                        <p>Email: ##Email##</p>
                        <p>Password: ##Password##</p>
                        <p>&nbsp;</p>
                        <p>Once you have logged in, you can change your password by clicking on the <b>Settings</b> menu button</p><p>&nbsp;</p>
                        <p>Should you have any queries, do not hesitate to contact us at ##ChurchName## on ##ChurchOfficeNo## or ##ChurchOfficeEmail##</p>
                        <p>&nbsp;</p>
                        <p>Enjoy</p>
                        <p>&nbsp;</p>
                     </div>
                 </div>
             </div>
         </div>
     <p >&nbsp;    </p>
 </div>
</body>
</html>
' from church

insert into ChurchEmailTemplate (ChurchId, EmailTemplateId, Template)
select churchid, 1, '<html>
<head>
    <title>Welcome to ##ChurchName## </title>
    <style type="text/css">
        @font-face
        {
            font-family:Calibri
        }
        
        a:link, span.MsoHyperlink
        {
            color:#2F4419;
            text-decoration:none none;
        }
        
        p
        {
            margin-top:0cm;
            margin-right:0cm;
            margin-bottom:0cm;
            margin-left:0pt;
            margin-bottom:.0001pt;
            text-align:justify;
            font-size:14.0pt;
            font-family:"Calibri";
            color:#043144;
            }
        li
        {
            font-size:14.0pt;
            font-family:"Calibri";
            color:#043144;
            text-align:left;
            }
    </style>
</head>
<body style="background-color: #FFFFFF">
    <div>
        <div align="left">
         
   <div style="display:table">
                <div style="display:table-row">
                    <div style="display:table-cell; width:12.75pt;padding:0cm 0cm 0cm 0cm">
                    </div>
                </div>
                <div style="display:table-row">
                    <div style="display:table-cell; width:12.75pt;padding:0cm 0cm 0cm 0cm">
                    </div>                
                    <div style="display:table-cell; width:537.75pt;padding:0cm 0cm 0cm 0cm">
                        <p>
                            <span style="font-size:14.0pt">Hi ##Firstname## ##Surname##</span>&nbsp;
                        </p>
                        <p>&nbsp;</p>
                        <p>Thank you for visiting us at ##ChurchName##.  We''d love to get to know you better, and let you experience different aspects and expressions of church life.  There is more information on our website <a href=''##ChurchWebsite##''>##ChurchWebsite##</a>.  We also meet in groups during the week and that''s a great place to meet people in the church.  You can find out more about the groups on the website, and we encourage you to visit one close to you.
                        <p>&nbsp;</p>
                        <p>This is also to let you know that you now have access to ##SystemName##.  This is a fun new way of keeping in touch with people in the church.  You''ll be able to <ul><li>update your details</li><li>lookup other people''s details</li><li>see what events/birthdays/anniversaries are about to happen</li></ul>
                        </p>
                        <p>##SystemName## is completely private and safe and your details are not shared with anyone outside the church.  Even if you log in using facebook - nothing is sent to facebook from ##SystemName##.  It simply uses facebook to check that you are who you say you are and it fetches your profile picture so people who are good with faces and not names can remember you.
                        <p>&nbsp;</p>
                        <p>In order to access it, go to <a href="https://www.oikonomos.co.za/Home/Login/##PublicId##">www.oikonomos.co.za</a> and do one of the following:</p>
                        <ul><li>If you have a facebook account you can click on the <b>Login using facebook button</b>.  This will then use your facebook username and password to connect you to the system.  It saves you remembering another password, and gives you additional functionality on ##SystemName##</li>
                        <li>If not, you can login with the following credentials</li></ul>
                        <p>Email: ##Email##</p>
                        <p>Password: ##Password##</p>
                        <p>&nbsp;</p>
                        <p>Once you have logged in, you can change your password by clicking on the <b>Settings</b> menu button</p><p>&nbsp;</p>
                        <p>Should you have any queries, do not hesitate to contact us at ##ChurchName## on ##ChurchOfficeNo## or ##ChurchOfficeEmail##</p>
                        <p>&nbsp;</p>
                        <p>Looking forward to seeing you again soon</p>
                        <p>&nbsp;</p>
                     </div>
                 </div>
             </div>
         </div>
     <p >&nbsp;    </p>
 </div>
</body>
</html>
' from church
