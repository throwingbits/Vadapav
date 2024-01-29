using Vadapav.Models.Http;

namespace Vadapav.Models
{
    internal static class VadapavModelFactory
    {
        internal static VadapavSearchResults WrapAsSearchResults(this List<VadapavDataItem> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            var result = new VadapavSearchResults
            {
                Count = entities.Count,
            };

            foreach (var entity in entities )
            {
                if (entity.IsDirectory)
                {
                    result.Directories.Add(entity.AsDirectory());
                }
                else
                {
                    result.Files.Add(entity.AsFile());
                }
            }

            return result;
        }

        internal static VadapavDirectory AsDirectory(this VadapavDataItem entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (!entity.IsDirectory)
                throw new InvalidOperationException($"Vadapav data item '{entity.Id}' is not a directory!");

            var directory = new VadapavDirectory
            {
                Id = entity.Id,
                Name = entity.Name,
                Parent = entity.Parent,
                ModifiedAt = entity.ModifiedAt,
            };

            if (entity.Files == null)
                return directory;

            directory.Files = entity
                .Files
                .Where(di => !di.IsDirectory)
                .Select(di => di.AsFile())
                .ToList();

            directory.Directories = entity
                .Files
                .Where(di => di.IsDirectory)
                .Select(di => di.AsDirectory())
                .ToList();

            return directory;
        }

        internal static VadapavFile AsFile(this VadapavDataItem entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (entity.IsDirectory)
                throw new InvalidOperationException($"Vadapav data item '{entity.Id}' is not a file!");

            var file = new VadapavFile
            {
                Id = entity.Id,
                Name = entity.Name,
                Parent = entity.Parent,
                ModifiedAt = entity.ModifiedAt,
                Size = entity.Size != null ? entity.Size.Value : 0,
            };

            return file;
        }
    }
}
