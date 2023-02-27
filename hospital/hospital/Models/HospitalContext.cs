using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace hospital.Models;

public partial class HospitalContext : DbContext
{
    public HospitalContext()
    {
    }

    public HospitalContext(DbContextOptions<HospitalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Clinic> Clinics { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Hospital;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clinic>(entity =>
        {
            entity.HasKey(e => e.ClincId).HasName("PK__Clinic__6E0653B916357CE6");

            entity.ToTable("Clinic");

            entity.Property(e => e.ClincId).HasColumnName("Clinc_ID");
            entity.Property(e => e.ClincName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Clinc_Name");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DocId).HasName("PK__Doctor__46473821C727FB58");

            entity.ToTable("Doctor");

            entity.Property(e => e.DocId).HasColumnName("Doc_ID");
            entity.Property(e => e.ClincId).HasColumnName("Clinc_ID");
            entity.Property(e => e.DocImg)
                .HasColumnType("text")
                .HasColumnName("Doc_Img");
            entity.Property(e => e.DocName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Doc_Name");

            entity.HasOne(d => d.Clinc).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.ClincId)
                .HasConstraintName("FK__Doctor__Clinc_ID__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
