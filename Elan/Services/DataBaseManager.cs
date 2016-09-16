using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Elan.Content;
using Elan.Models.Domain;
using Elan.Models.Implementations.Containers;
using DomainDocument = Elan.Models.Domain.Document;

namespace Elan.Services
{
    public static class DataBaseManager
    {
        public static void InitDataBase()
        {
            var connectionString = SettingsManager.LoadDbSettings().ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(Queries.InitDataBase);
                connection.Execute(Queries.InitTables);
            }
            SettingsManager.ReplaceDefaultDbName();
        }

        public static List<DomainDocument> GetDocumentNames()
        {
            var connectionString = SettingsManager.LoadDbSettings().ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<DomainDocument>(Queries.GetDocumentNames).ToList();
            }
        }

        public static bool IsDocumentExists(string name)
        {
            var connectionString = SettingsManager.LoadDbSettings().ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<bool>(Queries.IsDocumentExists, new {name}).FirstOrDefault();
            }
        }

        public static DomainDocument GetDocumentById(int id)
        {
            var connectionString = SettingsManager.LoadDbSettings().ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var document = connection.Query<DomainDocument>(Queries.GetDocumentById, new {id}).FirstOrDefault();

                if (document != null)
                {
                    document.Nodes = connection.Query<Node>(Queries.GetNodesByDocumentId,
                        new { documentId = document.Id }).ToList();

                    document.Links = connection.Query<Link> (Queries.GetLinksByDocumentId,
                        new { documentId = document.Id }).ToList();
                }

                return document;
            }
        }

        public static void SaveDocument(DomainDocument document)
        {
            var connectionString = SettingsManager.LoadDbSettings().ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {
                var documentId = connection.Query<int?>(Queries.DocumentSave, document).FirstOrDefault() ?? 0;
                if (documentId > 0)
                {
                    document.Nodes.ForEach(n => n.DocumentId = documentId);
                    document.Links.ForEach(l => l.DocumentId = documentId);
                    connection.Execute(Queries.DeleteNodesByDocumentId, new {documentId});
                    connection.Execute(Queries.DeleteLinksByDocumentId, new {documentId});
                    connection.Execute(Queries.NodeSave, document.Nodes);
                    connection.Execute(Queries.LinkSave, document.Links);
                }
            }
        }
    }
}