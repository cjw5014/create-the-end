// User Variables
string[] nameOfShipCargoContainers = new string[3] { "TorzMinerStorage 1", "TorzMinerStorage 2", "TorzMinerStorage 3" };
string statusDisplayName = "LCD Ore Offloader";
string nameOfOreDump = "Mars Alpha Largo Cargo 1";

public Program()
{

}

public void Main(string argument, UpdateType updateSource)
{

    var oreSummaries = new Dictionary<string, float>();
    IMyTerminalBlock oreDump = null;

    IMyTextPanel statusPanel = (IMyTextPanel)GridTerminalSystem.GetBlockWithName(statusDisplayName);
    // Simple useless call to ensure this exists. This will purposely trigger a null exception.
    statusPanel.GetText();

    oreDump = GridTerminalSystem.GetBlockWithName(nameOfOreDump);
    if (oreDump == null) {
        SetErrorText(statusPanel, $"Unable to find ore container: {nameOfOreDump}");
        return;
    }
    if (!oreDump.HasInventory) {
        SetErrorText(statusPanel, $"Target ore container: {nameOfOreDump} did not have inventory");
        return;
    }

    foreach (var containerName in nameOfShipCargoContainers) {
        var container = GridTerminalSystem.GetBlockWithName(containerName);
        if (container == null) {
            SetErrorText(statusPanel, $"Unable to ship ore container: {containerName}");
            return;
        }
        if (!container.HasInventory) {
            SetErrorText(statusPanel, $"Ship container: {containerName} did not have inventory");
            return;
        }

        MoveOreToTarget(oreSummaries, container, oreDump);
    }

    SetSummaryIfRequired(statusPanel, oreSummaries);
}

private void MoveOreToTarget(Dictionary<string, float> oreSummaries, IMyTerminalBlock container, IMyTerminalBlock targetContainer)
{
    for(var i = 0; i < container.InventoryCount; i++) {
        var inventory = container.GetInventory(i);
        for (var j = 0; j < inventory.ItemCount; j++) {
            var item = inventory.GetItemAt(j);
            if (item.HasValue) {
                var oreType = item.Value.Type.ToString();
                if (oreType.Contains("MyObjectBuilder_Ore")) {
                    var success = inventory.TransferItemTo(targetContainer.GetInventory(0), item.Value, null);
                    if (success) {
                        var oreName = oreType.Split('/')[1];
                        if (!oreSummaries.ContainsKey(oreName))
                        {
                            oreSummaries.Add(oreName, 0.0f);
                        }
                        oreSummaries[oreName] += (float)item.Value.Amount;
                    }
                }
            }
        }
    }
}

private void SetSummaryIfRequired(IMyTextPanel statusLcd, Dictionary<string, float> oreSummaries)
{
    if (oreSummaries.Count > 0)
    {
        var sb = new StringBuilder();
        sb.Append("Last Offload Summary...\n");
        foreach(var oreSummary in oreSummaries)
        {
            sb.Append($"{oreSummary.Key}: {oreSummary.Value} kg\n");
        }

        statusLcd.FontColor = new Color(255, 126, 52, 255); 
        statusLcd.FontSize = 0.8f;
        statusLcd.WriteText(sb.ToString(), false);
    }
}

private void SetErrorText(IMyTextPanel statusLcd, string error)
{
    statusLcd.FontColor = new Color(255, 0, 0, 255); 
    statusLcd.FontSize = 0.65f;
    statusLcd.WriteText(error, false);
}

