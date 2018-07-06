using ArchBook.Data.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArchBook.Data
{
    [DbConfigurationType(typeof(DbConfig))]
    public class BookDbContext : DbContext
    {
        public BookDbContext()
            : base("ArchBookConnectionString")
        {
        }

        public BookDbContext(string connectionString)
            : base(connectionString)
        {
        }

        static BookDbContext()
        {
            Database.SetInitializer<BookDbContext>(new CreateDatabaseIfNotExists<BookDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Book>()
                .HasOptional(x => x.Promotion)
                .WithRequired();

            modelBuilder.Entity<Book>()
                .HasMany(x => x.Authors)
                .WithMany(x => x.Books)
                .Map(x => x.ToTable("AuthorBook")
                           .MapLeftKey("BookId")
                           .MapRightKey("AuthorId"));
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw GetImprovedDbEntityValidationException(ex);
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException ex)
            {
                throw GetImprovedDbEntityValidationException(ex);
            }            
        }
        
        /// <summary>
        /// Generate a new DbEntityValidationException with improved exception message that include entities errors.
        /// </summary>
        /// <param name="ex">The original DbEntityValidationException.</param>
        /// <returns></returns>
        private DbEntityValidationException GetImprovedDbEntityValidationException(DbEntityValidationException ex)
        {
            var sb = new StringBuilder();

            foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
            {
                // obtenir les valeurs des clés primaires de l'entité en erreur.
                var keyValues = (from property in validationResult.Entry.Entity.GetType().GetProperties()
                                 where Attribute.IsDefined(property, typeof(KeyAttribute))
                                 orderby ((ColumnAttribute)property.GetCustomAttributes(false).SingleOrDefault(attr => attr is ColumnAttribute))?.Order ascending
                                 select string.Format("{0} = {1}", property.Name, property.GetValue(validationResult.Entry.Entity).ToString())).ToList();

                var keyValuesMessage = string.Join(", ", keyValues);
                var entityType = ObjectContext.GetObjectType(validationResult.Entry.Entity.GetType());
                var entityName = entityType.FullName;

                sb.AppendLine();
                sb.AppendFormat("The entity of type '{0}' with key(s) [{1}] in state '{2}' has the following validation errors: ", entityName, keyValuesMessage, validationResult.Entry.State);
                sb.Append(string.Join(" ", validationResult.ValidationErrors.Select(x => x.ErrorMessage)));
            }

            // Combine the original exception message with the new one.
            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", sb.ToString());

            // Throw a new DbEntityValidationException with the improved exception message.
            return new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        }

        public IDbSet<Book> Books { get; set; }

        public IDbSet<Author> Authors { get; set; }

        public IDbSet<PriceOffer> PricesOffers { get; set; }
    }
}
