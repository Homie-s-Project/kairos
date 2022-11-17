DROP TABLE IF EXISTS "public"."CompanionItem";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."CompanionItem" (
    "CompanionsCompanionId" int4 NOT NULL,
    "ItemsItemId" int4 NOT NULL,
    CONSTRAINT "FK_CompanionItem_Items_ItemsItemId" FOREIGN KEY ("ItemsItemId") REFERENCES "public"."Items"("ItemId") ON DELETE CASCADE,
    CONSTRAINT "FK_CompanionItem_Companions_CompanionsCompanionId" FOREIGN KEY ("CompanionsCompanionId") REFERENCES "public"."Companions"("CompanionId") ON DELETE CASCADE,
    PRIMARY KEY ("CompanionsCompanionId","ItemsItemId")
);

DROP TABLE IF EXISTS "public"."Companions";
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

DROP TABLE IF EXISTS "public"."EventLabel";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."EventLabel" (
    "EventsEventId" int4 NOT NULL,
    "LabelsLabelId" int4 NOT NULL,
    CONSTRAINT "FK_EventLabel_Events_EventsEventId" FOREIGN KEY ("EventsEventId") REFERENCES "public"."Events"("EventId") ON DELETE CASCADE,
    CONSTRAINT "FK_EventLabel_Labels_LabelsLabelId" FOREIGN KEY ("LabelsLabelId") REFERENCES "public"."Labels"("LabelId") ON DELETE CASCADE,
    PRIMARY KEY ("EventsEventId","LabelsLabelId")
);

DROP TABLE IF EXISTS "public"."Events";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Events" (
    "EventId" int4 NOT NULL,
    "EventDate" timestamptz NOT NULL,
    "EventTitle" text,
    "EventDescription" text,
    "EventCreatedDate" timestamptz NOT NULL DEFAULT '-infinity'::timestamp with time zone,
    PRIMARY KEY ("EventId")
);

DROP TABLE IF EXISTS "public"."GroupLabel";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."GroupLabel" (
    "GroupsGroupId" int4 NOT NULL,
    "LabelsLabelId" int4 NOT NULL,
    CONSTRAINT "FK_GroupLabel_Groups_GroupsGroupId" FOREIGN KEY ("GroupsGroupId") REFERENCES "public"."Groups"("GroupId") ON DELETE CASCADE,
    CONSTRAINT "FK_GroupLabel_Labels_LabelsLabelId" FOREIGN KEY ("LabelsLabelId") REFERENCES "public"."Labels"("LabelId") ON DELETE CASCADE,
    PRIMARY KEY ("GroupsGroupId","LabelsLabelId")
);

DROP TABLE IF EXISTS "public"."Groups";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Groups" (
    "GroupId" int4 NOT NULL,
    "GroupName" text,
    "EventId" int4 DEFAULT 0,
    "GroupsIsPrivate" bool NOT NULL DEFAULT false,
    "UserId" int4 NOT NULL DEFAULT 0,
    CONSTRAINT "FK_Groups_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "public"."Events"("EventId"),
    PRIMARY KEY ("GroupId")
);

DROP TABLE IF EXISTS "public"."GroupUser";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."GroupUser" (
    "GroupsGroupId" int4 NOT NULL,
    "UsersUserId" int4 NOT NULL,
    CONSTRAINT "FK_GroupUser_Groups_GroupsGroupId" FOREIGN KEY ("GroupsGroupId") REFERENCES "public"."Groups"("GroupId") ON DELETE CASCADE,
    CONSTRAINT "FK_GroupUser_Users_UsersUserId" FOREIGN KEY ("UsersUserId") REFERENCES "public"."Users"("UserId") ON DELETE CASCADE,
    PRIMARY KEY ("GroupsGroupId","UsersUserId")
);

DROP TABLE IF EXISTS "public"."Items";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Items" (
    "ItemId" int4 NOT NULL,
    "ItemName" text,
    "ItemType" int4 NOT NULL,
    PRIMARY KEY ("ItemId")
);

DROP TABLE IF EXISTS "public"."Labels";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Labels" (
    "LabelId" int4 NOT NULL,
    "LabelTitle" text,
    "UserId" int4 NOT NULL,
    CONSTRAINT "FK_Labels_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."Users"("UserId") ON DELETE CASCADE,
    PRIMARY KEY ("LabelId")
);

DROP TABLE IF EXISTS "public"."LabelStudies";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."LabelStudies" (
    "LabelsLabelId" int4 NOT NULL,
    "StudiesId" int4 NOT NULL,
    CONSTRAINT "FK_LabelStudies_Labels_LabelsLabelId" FOREIGN KEY ("LabelsLabelId") REFERENCES "public"."Labels"("LabelId") ON DELETE CASCADE,
    CONSTRAINT "FK_LabelStudies_Studies_StudiesId" FOREIGN KEY ("StudiesId") REFERENCES "public"."Studies"("StudiesId") ON DELETE CASCADE,
    PRIMARY KEY ("LabelsLabelId","StudiesId")
);

DROP TABLE IF EXISTS "public"."OAuth2Credentials";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."OAuth2Credentials" (
    "Id" int4 NOT NULL,
    "AccessToken" text,
    "UserId" int4 NOT NULL,
    CONSTRAINT "FK_OAuth2Credentials_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."Users"("UserId") ON DELETE CASCADE,
    PRIMARY KEY ("Id")
);

DROP TABLE IF EXISTS "public"."Reminders";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Reminders" (
    "ReminderId" int4 NOT NULL,
    "ReminderTime" timestamptz NOT NULL,
    "StudiesId" int4 NOT NULL,
    CONSTRAINT "FK_Reminders_Studies_StudiesId" FOREIGN KEY ("StudiesId") REFERENCES "public"."Studies"("StudiesId") ON DELETE CASCADE,
    PRIMARY KEY ("ReminderId")
);

DROP TABLE IF EXISTS "public"."Studies";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Table Definition
CREATE TABLE "public"."Studies" (
    "StudiesId" int4 NOT NULL,
    "StudiesNumber" text,
    "StudiesTime" text,
    "GroupId" int4 NOT NULL,
    "StudiesCreatedDate" timestamptz NOT NULL DEFAULT '-infinity'::timestamp with time zone,
    CONSTRAINT "FK_Studies_Groups_GroupId" FOREIGN KEY ("GroupId") REFERENCES "public"."Groups"("GroupId") ON DELETE CASCADE,
    PRIMARY KEY ("StudiesId")
);

DROP TABLE IF EXISTS "public"."Users";
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
    "ServiceId" text,
    PRIMARY KEY ("UserId")
);









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
