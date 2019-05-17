namespace HospitalImplementations.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Medications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MedicationRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicationId = c.Int(nullable: false),
                        RequestId = c.Int(nullable: false),
                        CountMedications = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medications", t => t.MedicationId, cascadeDelete: true)
                .ForeignKey("dbo.Requests", t => t.RequestId, cascadeDelete: true)
                .Index(t => t.MedicationId)
                .Index(t => t.RequestId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PrescriptionMedications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrescriptionId = c.Int(nullable: false),
                        MedicationId = c.Int(nullable: false),
                        CountMedications = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medications", t => t.MedicationId, cascadeDelete: true)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.PrescriptionId)
                .Index(t => t.MedicationId);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TreatmentPrescriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TreatmentId = c.Int(nullable: false),
                        PrescriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .ForeignKey("dbo.Treatments", t => t.TreatmentId, cascadeDelete: true)
                .Index(t => t.TreatmentId)
                .Index(t => t.PrescriptionId);
            
            CreateTable(
                "dbo.Treatments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        TotalCost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FIO = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreatmentPrescriptions", "TreatmentId", "dbo.Treatments");
            DropForeignKey("dbo.Treatments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.TreatmentPrescriptions", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.PrescriptionMedications", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.PrescriptionMedications", "MedicationId", "dbo.Medications");
            DropForeignKey("dbo.MedicationRequests", "RequestId", "dbo.Requests");
            DropForeignKey("dbo.MedicationRequests", "MedicationId", "dbo.Medications");
            DropIndex("dbo.Treatments", new[] { "PatientId" });
            DropIndex("dbo.TreatmentPrescriptions", new[] { "PrescriptionId" });
            DropIndex("dbo.TreatmentPrescriptions", new[] { "TreatmentId" });
            DropIndex("dbo.PrescriptionMedications", new[] { "MedicationId" });
            DropIndex("dbo.PrescriptionMedications", new[] { "PrescriptionId" });
            DropIndex("dbo.MedicationRequests", new[] { "RequestId" });
            DropIndex("dbo.MedicationRequests", new[] { "MedicationId" });
            DropTable("dbo.Patients");
            DropTable("dbo.Treatments");
            DropTable("dbo.TreatmentPrescriptions");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.PrescriptionMedications");
            DropTable("dbo.Requests");
            DropTable("dbo.MedicationRequests");
            DropTable("dbo.Medications");
        }
    }
}
