using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Robust.Server.GameObjects;
using Robust.Shared.ContentPack;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Map.Events;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Utility;

namespace Content.Server.Maps;

/// <summary>
///     Performs basic map migration operations by listening for engine <see cref="MapLoaderSystem"/> events.
/// </summary>
public sealed class MapMigrationSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _protoMan = default!;
    [Dependency] private readonly IResourceManager _resMan = default!;

    private static readonly string[] MigrationFiles = ["/migration.yml", "/starshine_migration.yml"];

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BeforeEntityReadEvent>(OnBeforeReadEvent);

#if DEBUG
        if (!TryReadFiles(out var mappingsList))
            return;

        // Verify that all of the entries map to valid entity prototypes.
        foreach (var mappings in mappingsList)
        {
            foreach (var node in mappings.Values)
            {
                var newId = ((ValueDataNode) node).Value;
                if (!string.IsNullOrEmpty(newId) && newId != "null")
                    DebugTools.Assert(_protoMan.HasIndex<EntityPrototype>(newId), $"{newId} is not an entity prototype.");
            }
        }
#endif
    }

    private bool TryReadFiles([NotNullWhen(true)] out List<MappingDataNode>? mappingsList)
    {
        mappingsList = new List<MappingDataNode>();

        foreach (var file in MigrationFiles)
        {
            var path = new ResPath(file);
            if (!_resMan.TryContentFileRead(path, out var stream))
                continue;

            using var reader = new StreamReader(stream, EncodingHelpers.UTF8);
            var documents = DataNodeParser.ParseYamlStream(reader).FirstOrDefault();

            if (documents == null)
                continue;

            mappingsList.Add((MappingDataNode) documents.Root);
        }

        return mappingsList.Count > 0;
    }

    private void OnBeforeReadEvent(BeforeEntityReadEvent ev)
    {
        if (!TryReadFiles(out var mappingsList))
            return;

        foreach (var mappings in mappingsList)
        {
            foreach (var (key, value) in mappings)
            {
                if (key is not ValueDataNode keyNode || value is not ValueDataNode valueNode)
                    continue;

                if (string.IsNullOrWhiteSpace(valueNode.Value) || valueNode.Value == "null")
                    ev.DeletedPrototypes.Add(keyNode.Value);
                else
                    ev.RenamedPrototypes.Add(keyNode.Value, valueNode.Value);
            }
        }
    }
}
