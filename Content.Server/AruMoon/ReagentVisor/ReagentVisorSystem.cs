using Robust.Shared.Prototypes;
using Content.Shared.Chemistry.Components;
using Content.Shared.Examine;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Inventory;

namespace Content.Server.ReagentVisor
{
    /// <summary>
    /// ""
    /// </summary>
    public sealed class ReagentVisorSystem : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly InventorySystem _inventorySystem = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<InventoryComponent, ReagentVisorAttemptEvent>(OnInventoryReagentVisorAttempt);
            SubscribeLocalEvent<ReagentVisorComponent, ReagentVisorAttemptEvent>(OnReagentVisorAttempt);
        }

        public void PushScanMarkup(Solution? solution, ExaminedEvent examEvent)
        {
            if (solution == null ||
                solution.Contents.Count == 0  
            ) return;

            var text = Loc.GetString("science-goggles-examine-header");
            
            foreach (var reagent in solution.Contents)
            {
                if (!_prototypeManager.TryIndex(reagent.ReagentId, out ReagentPrototype? proto)) continue;
                
                // Increasing color brightness to make dark reagents text readable
                var color = Color.ToHsv(proto.SubstanceColor);
                color.Z = color.Z < 0.6f ? 0.6f : color.Z;

                text += "\n" + Loc.GetString("science-goggles-examine-element",
                    ("reagent", proto.LocalizedName),
                    ("color", Color.FromHsv(color).ToHexNoAlpha()),
                    ("quantity", reagent.Quantity)
                );
            }
            examEvent.PushMarkup(text);
        }

        private void OnInventoryReagentVisorAttempt(EntityUid uid, InventoryComponent component, ReagentVisorAttemptEvent args)
        {
            foreach (var slot in new string[]{"head", "eyes", "mask"})
            {
                if (args.Handled)
                    break;
                if (_inventorySystem.TryGetSlotEntity(uid, slot, out var item, component))
                    RaiseLocalEvent(item.Value, args);
            }
        }

        private void OnReagentVisorAttempt(EntityUid uid, ReagentVisorComponent component, ReagentVisorAttemptEvent args)
        {
            PushScanMarkup(args.Solution, args.ExaminedEvent);
            args.Handled = true;
        }

        public sealed class ReagentVisorAttemptEvent : HandledEntityEventArgs
        {   
            public ExaminedEvent ExaminedEvent;
            public Solution? Solution;

            public ReagentVisorAttemptEvent(Solution? solution, ExaminedEvent exEvent)
            {
                Solution = solution;
                ExaminedEvent = exEvent;
            }
        }
    }
}