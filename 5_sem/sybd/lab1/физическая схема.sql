CREATE TABLE "cases" (
  "case_id" integer PRIMARY KEY NOT NULL,
  "case_number" varchar(30) UNIQUE NOT NULL,
  "registration_date" date NOT NULL,
  "status" varchar(20) NOT NULL,
  "description" text
);

CREATE TABLE "employees" (
  "employee_id" integer PRIMARY KEY NOT NULL,
  "full_name" varchar(150) NOT NULL,
  "position" varchar(50) NOT NULL,
  "specialization" varchar(100),
  "hire_date" date NOT NULL
);

CREATE TABLE "evidence" (
  "evidence_id" integer PRIMARY KEY NOT NULL,
  "case_id" integer NOT NULL,
  "evidence_number" varchar(30) UNIQUE NOT NULL,
  "seizure_date" date NOT NULL,
  "seizure_location" varchar(200) NOT NULL,
  "description" text
);

CREATE TABLE "storage_devices" (
  "device_id" integer PRIMARY KEY NOT NULL,
  "evidence_id" integer NOT NULL,
  "device_type" varchar(30) NOT NULL,
  "serial_number" varchar(50) UNIQUE NOT NULL,
  "brand" varchar(50),
  "model" varchar(50),
  "capacity_gb" numeric(10,2)
);

CREATE TABLE "digital_files" (
  "file_id" integer PRIMARY KEY NOT NULL,
  "device_id" integer NOT NULL,
  "file_name" varchar(255) NOT NULL,
  "file_hash_sha256" char(64) UNIQUE NOT NULL,
  "file_size_bytes" bigint NOT NULL,
  "file_type" varchar(30) NOT NULL,
  "discovery_date" date NOT NULL
);

CREATE TABLE "examinations" (
  "examination_id" integer PRIMARY KEY NOT NULL,
  "case_id" integer NOT NULL,
  "employee_id" integer NOT NULL,
  "appointment_date" date NOT NULL,
  "start_date" date,
  "end_date" date,
  "conclusion" text
);

CREATE TABLE "analysis_objects" (
  "examination_id" integer NOT NULL,
  "file_id" integer NOT NULL,
  "analysis_result" varchar(100) NOT NULL,
  "analysis_date" date NOT NULL,
  PRIMARY KEY ("examination_id", "file_id")
);

COMMENT ON TABLE "examinations" IS 'CHECK (appointment_date <= start_date OR start_date IS NULL)';

ALTER TABLE "evidence" ADD FOREIGN KEY ("case_id") REFERENCES "cases" ("case_id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "storage_devices" ADD FOREIGN KEY ("evidence_id") REFERENCES "evidence" ("evidence_id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "digital_files" ADD FOREIGN KEY ("device_id") REFERENCES "storage_devices" ("device_id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "examinations" ADD FOREIGN KEY ("case_id") REFERENCES "cases" ("case_id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "examinations" ADD FOREIGN KEY ("employee_id") REFERENCES "employees" ("employee_id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "analysis_objects" ADD FOREIGN KEY ("examination_id") REFERENCES "examinations" ("examination_id") DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "analysis_objects" ADD FOREIGN KEY ("file_id") REFERENCES "digital_files" ("file_id") DEFERRABLE INITIALLY IMMEDIATE;
