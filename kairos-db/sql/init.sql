-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    PRIMARY KEY ("MigrationId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."CompanionItem" (
    "CompanionsCompanionId" int4 NOT NULL,
    "ItemsItemId" int4 NOT NULL,
    CONSTRAINT "FK_CompanionItem_Items_ItemsItemId" FOREIGN KEY ("ItemsItemId") REFERENCES "public"."Items"("ItemId") ON DELETE CASCADE,
    CONSTRAINT "FK_CompanionItem_Companions_CompanionsCompanionId" FOREIGN KEY ("CompanionsCompanionId") REFERENCES "public"."Companions"("CompanionId") ON DELETE CASCADE,
    PRIMARY KEY ("CompanionsCompanionId","ItemsItemId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Companions" (
    "CompanionId" int4 NOT NULL,
    "CompanionName" text,
    "CompanionType" int4 NOT NULL,
    "CompanionAge" int4 NOT NULL,
    "UserId" int4 NOT NULL DEFAULT 0,
    CONSTRAINT "FK_Companions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."Users"("UserId") ON DELETE CASCADE,
    PRIMARY KEY ("CompanionId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."EventLabel" (
    "EventsEventId" int4 NOT NULL,
    "LabelsLabelId" int4 NOT NULL,
    CONSTRAINT "FK_EventLabel_Events_EventsEventId" FOREIGN KEY ("EventsEventId") REFERENCES "public"."Events"("EventId") ON DELETE CASCADE,
    CONSTRAINT "FK_EventLabel_Labels_LabelsLabelId" FOREIGN KEY ("LabelsLabelId") REFERENCES "public"."Labels"("LabelId") ON DELETE CASCADE,
    PRIMARY KEY ("EventsEventId","LabelsLabelId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Events" (
    "EventId" int4 NOT NULL,
    "EventDate" timestamptz NOT NULL,
    "EventTitle" text,
    "EventDescription" text,
    PRIMARY KEY ("EventId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."GroupLabel" (
    "GroupsGroupId" int4 NOT NULL,
    "LabelsLabelId" int4 NOT NULL,
    CONSTRAINT "FK_GroupLabel_Groups_GroupsGroupId" FOREIGN KEY ("GroupsGroupId") REFERENCES "public"."Groups"("GroupId") ON DELETE CASCADE,
    CONSTRAINT "FK_GroupLabel_Labels_LabelsLabelId" FOREIGN KEY ("LabelsLabelId") REFERENCES "public"."Labels"("LabelId") ON DELETE CASCADE,
    PRIMARY KEY ("GroupsGroupId","LabelsLabelId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Groups" (
    "GroupId" int4 NOT NULL,
    "GroupName" text,
    "EventId" int4,
    "GroupsIsPrivate" bool NOT NULL DEFAULT false,
    "UserId" int4 NOT NULL DEFAULT 0,
    CONSTRAINT "FK_Groups_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "public"."Events"("EventId"),
    CONSTRAINT "FK_Groups_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."Users"("UserId") ON DELETE CASCADE,
    PRIMARY KEY ("GroupId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Items" (
    "ItemId" int4 NOT NULL,
    "ItemName" text,
    "ItemType" int4 NOT NULL,
    PRIMARY KEY ("ItemId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Labels" (
    "LabelId" int4 NOT NULL,
    "LabelTitle" text,
    "UserId" int4 NOT NULL,
    CONSTRAINT "FK_Labels_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."Users"("UserId") ON DELETE CASCADE,
    PRIMARY KEY ("LabelId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."LabelStudies" (
    "LabelsLabelId" int4 NOT NULL,
    "StudiesId" int4 NOT NULL,
    CONSTRAINT "FK_LabelStudies_Labels_LabelsLabelId" FOREIGN KEY ("LabelsLabelId") REFERENCES "public"."Labels"("LabelId") ON DELETE CASCADE,
    CONSTRAINT "FK_LabelStudies_Studies_StudiesId" FOREIGN KEY ("StudiesId") REFERENCES "public"."Studies"("StudiesId") ON DELETE CASCADE,
    PRIMARY KEY ("LabelsLabelId","StudiesId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."OAuth2Credentials" (
    "Id" int4 NOT NULL,
    "AccessToken" text,
    "UserId" int4 NOT NULL,
    CONSTRAINT "FK_OAuth2Credentials_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."Users"("UserId") ON DELETE CASCADE,
    PRIMARY KEY ("Id")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Reminders" (
    "ReminderId" int4 NOT NULL,
    "ReminderTime" timestamptz NOT NULL,
    "StudiesId" int4 NOT NULL,
    CONSTRAINT "FK_Reminders_Studies_StudiesId" FOREIGN KEY ("StudiesId") REFERENCES "public"."Studies"("StudiesId") ON DELETE CASCADE,
    PRIMARY KEY ("ReminderId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Studies" (
    "StudiesId" int4 NOT NULL,
    "StudiesNumber" text,
    "StudiesTime" text,
    "GroupId" int4 NOT NULL,
    CONSTRAINT "FK_Studies_Groups_GroupId" FOREIGN KEY ("GroupId") REFERENCES "public"."Groups"("GroupId") ON DELETE CASCADE,
    PRIMARY KEY ("StudiesId")
);

-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Users" (
    "UserId" int4 NOT NULL,
    "LastName" text,
    "FirstName" text,
    "BirthDate" timestamptz NOT NULL,
    "Email" text,
    "CreatedAt" timestamptz NOT NULL,
    "LastUpdatedAt" timestamptz NOT NULL,
    "MicrosoftId" text,
    "GroupId" int4,
    CONSTRAINT "FK_Users_Groups_GroupId" FOREIGN KEY ("GroupId") REFERENCES "public"."Groups"("GroupId"),
    PRIMARY KEY ("UserId")
);

INSERT INTO "public"."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES
('20221020152531_InitialCreate', '6.0.10');
INSERT INTO "public"."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES
('20221024142509_Oauth2CredentialsCreate', '6.0.10');
INSERT INTO "public"."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES
('20221106134924_AllNeededModelsForMVP', '6.0.10');
INSERT INTO "public"."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES
('20221106135113_EventModelsRetireUserId', '6.0.10'),
('20221116180047_DatabaseTotalChange', '6.0.10'),
('20221116183032_DatabaseRenameCompanion', '6.0.10'),
('20221116183145_DatabaseUserCompanion', '6.0.10');

INSERT INTO "public"."CompanionItem" ("CompanionsCompanionId", "ItemsItemId") VALUES
(2, 2);
INSERT INTO "public"."CompanionItem" ("CompanionsCompanionId", "ItemsItemId") VALUES
(2, 1);


INSERT INTO "public"."Companions" ("CompanionId", "CompanionName", "CompanionType", "CompanionAge", "UserId") VALUES
(2, 'Rudolph', 1, 1, 1);






INSERT INTO "public"."GroupLabel" ("GroupsGroupId", "LabelsLabelId") VALUES
(1, 1);
INSERT INTO "public"."GroupLabel" ("GroupsGroupId", "LabelsLabelId") VALUES
(1, 3);


INSERT INTO "public"."Groups" ("GroupId", "GroupName", "EventId", "GroupsIsPrivate", "UserId") VALUES
(1, 'Romain_Group', NULL, 't', 2);


INSERT INTO "public"."Items" ("ItemId", "ItemName", "ItemType") VALUES
(1, 'Eau', 1);
INSERT INTO "public"."Items" ("ItemId", "ItemName", "ItemType") VALUES
(2, 'Chips', 2);
INSERT INTO "public"."Items" ("ItemId", "ItemName", "ItemType") VALUES
(4, 'Coca-cola', 1);
INSERT INTO "public"."Items" ("ItemId", "ItemName", "ItemType") VALUES
(5, 'Lego Technic', 3),
(6, 'Cars', 3),
(3, 'Beer', 1);

INSERT INTO "public"."Labels" ("LabelId", "LabelTitle", "UserId") VALUES
(1, 'Math', 1);
INSERT INTO "public"."Labels" ("LabelId", "LabelTitle", "UserId") VALUES
(2, 'Science', 1);
INSERT INTO "public"."Labels" ("LabelId", "LabelTitle", "UserId") VALUES
(3, 'Art', 1);
INSERT INTO "public"."Labels" ("LabelId", "LabelTitle", "UserId") VALUES
(4, 'Physique', 1),
(5, 'Informatique', 1);

INSERT INTO "public"."LabelStudies" ("LabelsLabelId", "StudiesId") VALUES
(1, 1);


INSERT INTO "public"."Reminders" ("ReminderId", "ReminderTime", "StudiesId") VALUES
(1, '2022-11-16 18:36:04.443222+00', 1);


INSERT INTO "public"."Studies" ("StudiesId", "StudiesNumber", "StudiesTime", "GroupId") VALUES
(1, '1', '24-11-2022', 1);


INSERT INTO "public"."Users" ("UserId", "LastName", "FirstName", "BirthDate", "Email", "CreatedAt", "LastUpdatedAt", "MicrosoftId", "GroupId") VALUES
(2, 'Romain', 'Antunes', '-infinity', 'romainantunes2003@hotmail.com', '2022-11-16 19:17:53.196636+00', '2022-11-16 19:17:53.196636+00', '0a89cb1d-d95f-4d20-bd6e-3808b182ed24', NULL);
INSERT INTO "public"."Users" ("UserId", "LastName", "FirstName", "BirthDate", "Email", "CreatedAt", "LastUpdatedAt", "MicrosoftId", "GroupId") VALUES
(1, 'LastName', 'FirstName', '2003-04-11 00:00:00+00', 'user@kairos.com', '2022-11-16 18:28:19.423281+00', '2022-11-16 18:28:19.423281+00', 'gfrewreg4t-3reqwfadsf3rfsea-fsdfas', 1);

