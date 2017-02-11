DROP TYPE IF EXISTS "EventTypes";
CREATE TYPE "EventTypes" as ENUM ('PatientActivated', 'PatientDeactivated');

DROP TABLE IF EXISTS "JsonEnumMap";

CREATE TABLE "JsonEnumMap" (
	"Id" SERIAL PRIMARY KEY NOT NULL,
	"Jsonb" JSONB NOT NULL,
	"Event" "EventTypes" NOT NULL
);