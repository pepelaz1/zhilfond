ALTER TABLE "ImpFiles" ADD COLUMN "Signed" boolean NOT NULL DEFAULT false;

UPDATE "ImpFiles"
   SET "Signed"=true;