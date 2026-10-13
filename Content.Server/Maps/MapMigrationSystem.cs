using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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
public sealed partial class MapMigrationSystem : EntitySystem
{
    [Dependency] private IResourceManager _resMan = default!;

    // Starshine-Changes: Added multi-migration file support
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
            foreach (var node in mappings.Children.Values)
            {
                var newId = ((ValueDataNode) node).Value;
                if (!string.IsNullOrEmpty(newId) && newId != "null")
                    DebugTools.Assert(ProtoMan.HasIndex<EntityPrototype>(newId), $"{newId} is not an entity prototype.");
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
                if (value is not ValueDataNode valueNode)
                    continue;

                if (string.IsNullOrWhiteSpace(valueNode.Value) || valueNode.Value == "null")
                    ev.DeletedPrototypes.Add(key);
                else
                    ev.RenamedPrototypes.Add(key, valueNode.Value);
            }
        }
    }
}
