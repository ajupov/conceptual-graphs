﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Elan.Content {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Queries {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Queries() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Elan.Content.Queries", typeof(Queries).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на delete from dbo.Link
        ///	where DocumentId = @documentId;.
        /// </summary>
        internal static string DeleteLinksByDocumentId {
            get {
                return ResourceManager.GetString("DeleteLinksByDocumentId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на delete from dbo.Node
        ///	where DocumentId = @documentId;.
        /// </summary>
        internal static string DeleteNodesByDocumentId {
            get {
                return ResourceManager.GetString("DeleteNodesByDocumentId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на if(not exists (
        ///	select
        ///		Id
        ///		from dbo.Document
        ///		where Name = @Name)
        ///	) begin
        ///insert into dbo.Document
        ///	values(@Name);
        ///	select scope_identity();
        ///end
        ///else begin
        ///	select Id from dbo.Document where Name = @Name;
        ///end;.
        /// </summary>
        internal static string DocumentSave {
            get {
                return ResourceManager.GetString("DocumentSave", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на select
        ///	Id,
        ///	Name
        ///	from dbo.Document
        ///	where Id = @id.
        /// </summary>
        internal static string GetDocumentById {
            get {
                return ResourceManager.GetString("GetDocumentById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на select
        ///	Id,
        ///	Name	
        ///	from dbo.Document;.
        /// </summary>
        internal static string GetDocumentNames {
            get {
                return ResourceManager.GetString("GetDocumentNames", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на select
        ///	Id,
        ///	DocumentId,
        ///	StartNodeId,
        ///	EndNodeId,
        ///	Label,
        ///	StartPointX,
        ///	StartPointY,
        ///	EndPointX,
        ///	EndPointY
        ///	from dbo.Link
        ///	where DocumentId = @documentId;.
        /// </summary>
        internal static string GetLinksByDocumentId {
            get {
                return ResourceManager.GetString("GetLinksByDocumentId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на select
        ///	Id,
        ///	DocumentId,
        ///	Type,
        ///	Label,
        ///	X,
        ///	Y,
        ///	Width,
        ///	Height
        ///	from dbo.Node
        ///	where DocumentId = @documentId;.
        /// </summary>
        internal static string GetNodesByDocumentId {
            get {
                return ResourceManager.GetString("GetNodesByDocumentId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на use [master];
        ///
        ///if(not exists (
        ///	select
        ///		[name]
        ///		from master.dbo.sysdatabases
        ///		where [name] = &apos;elan&apos;)) begin
        ///	create database elan;
        ///end;.
        /// </summary>
        internal static string InitDataBase {
            get {
                return ResourceManager.GetString("InitDataBase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на use elan;
        ///if (not exists (
        ///	select
        ///		TABLE_NAME
        ///        from information_schema.tables 
        ///        where TABLE_SCHEMA = &apos;dbo&apos; 
        ///			and TABLE_NAME = &apos;Document&apos;))
        ///begin
        ///	create table dbo.Document
        ///	(
        ///		Id		int identity (1, 1) not null,
        ///		Name	nvarchar(max) not null
        ///	);
        ///end;.
        /// </summary>
        internal static string InitTableDocument {
            get {
                return ResourceManager.GetString("InitTableDocument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на use elan;
        ///if (not exists (
        ///	select
        ///		TABLE_NAME
        ///        from information_schema.tables 
        ///        where TABLE_SCHEMA = &apos;dbo&apos; 
        ///			and TABLE_NAME = &apos;Link&apos;))
        ///begin
        ///	create table dbo.Link
        ///	(
        ///		Id			int not null,
        ///		DocumentId	int not null,
        ///		StartNodeId	int not null,
        ///		EndNodeId	int not null,
        ///		Label		nvarchar(max) not null,
        ///		StartPointX	int not null,
        ///		StartPointY	int not null,
        ///		EndPointX	int not null,
        ///		EndPointY	int not null
        ///	);
        ///end;.
        /// </summary>
        internal static string InitTableLink {
            get {
                return ResourceManager.GetString("InitTableLink", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на use elan;
        ///if (not exists (
        ///	select
        ///		TABLE_NAME
        ///        from information_schema.tables 
        ///        where TABLE_SCHEMA = &apos;dbo&apos; 
        ///			and TABLE_NAME = &apos;Node&apos;))
        ///begin
        ///	create table dbo.Node
        ///	(
        ///		Id			int not null,
        ///		DocumentId	int not null,
        ///		Type		int not null,
        ///		Label		nvarchar(max) not null,
        ///		X			int not null,
        ///		Y			int not null,
        ///		Width		int not null,
        ///		Height		int not null
        ///	);
        ///end;.
        /// </summary>
        internal static string InitTableNode {
            get {
                return ResourceManager.GetString("InitTableNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на select
        ///	convert(bit, iif(exists(
        ///		select
        ///			Id
        ///			from dbo.Document
        ///			where Name = @name), 1, 0));.
        /// </summary>
        internal static string IsDocumentExists {
            get {
                return ResourceManager.GetString("IsDocumentExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на insert into dbo.Link
        ///	values(@Id, @DocumentId, @StartNodeId, @EndNodeId, @Label, @StartPointX, @StartPointY, @EndPointX, @EndPointY);.
        /// </summary>
        internal static string LinkSave {
            get {
                return ResourceManager.GetString("LinkSave", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на insert into dbo.Node
        ///	values(@Id, @DocumentId, @Type, @Label, @X, @Y, @Width, @Height);.
        /// </summary>
        internal static string NodeSave {
            get {
                return ResourceManager.GetString("NodeSave", resourceCulture);
            }
        }
    }
}
