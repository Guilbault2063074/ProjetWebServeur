using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Projet_Web_Serveur.Models;

public partial class ProjetWsContext : DbContext
{
    public ProjetWsContext()
    {
    }

    public ProjetWsContext(DbContextOptions<ProjetWsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Choixdereponse> Choixdereponses { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<Quizquestion> Quizquestions { get; set; }

    public virtual DbSet<Totalquiz> Totalquizzes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=ConnectionStrings:MySqlConnection", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Choixdereponse>(entity =>
        {
            entity.HasKey(e => e.ChoixId).HasName("PRIMARY");

            entity.ToTable("choixdereponse");

            entity.HasIndex(e => e.QuestionId, "QuestionID");

            entity.Property(e => e.ChoixId)
                .ValueGeneratedNever()
                .HasColumnName("ChoixID");
            entity.Property(e => e.Choix).HasMaxLength(512);
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

            entity.HasOne(d => d.Question).WithMany(p => p.Choixdereponses)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("choixdereponse_ibfk_1");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PRIMARY");

            entity.ToTable("question");

            entity.Property(e => e.QuestionId)
                .ValueGeneratedNever()
                .HasColumnName("QuestionID");
            entity.Property(e => e.Question1)
                .HasMaxLength(512)
                .HasColumnName("Question");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.QuizId).HasName("PRIMARY");

            entity.ToTable("quiz");

            entity.HasIndex(e => e.Email, "Email");

            entity.Property(e => e.QuizId)
                .ValueGeneratedNever()
                .HasColumnName("QuizID");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.Email)
                .HasConstraintName("quiz_ibfk_1");
        });

        modelBuilder.Entity<Quizquestion>(entity =>
        {
            entity.HasKey(e => e.MyRowId).HasName("PRIMARY");

            entity.ToTable("quizquestions");

            entity.HasIndex(e => e.QuestionId, "QuestionID");

            entity.HasIndex(e => e.QuizId, "QuizID");

            entity.Property(e => e.MyRowId).HasColumnName("my_row_id");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.QuizId).HasColumnName("QuizID");

            entity.HasOne(d => d.Question).WithMany(p => p.Quizquestions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("quizquestions_ibfk_2");

            entity.HasOne(d => d.Quiz).WithMany(p => p.Quizquestions)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("quizquestions_ibfk_1");
        });

        modelBuilder.Entity<Totalquiz>(entity =>
        {
            entity.HasKey(e => e.MyRowId).HasName("PRIMARY");

            entity.ToTable("totalquiz");

            entity.Property(e => e.MyRowId).HasColumnName("my_row_id");
            entity.Property(e => e.Totalquiz1).HasColumnName("totalquiz");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
