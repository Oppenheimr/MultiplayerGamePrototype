Assets

DoTween : https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676
Starter Pack - Synty POLYGON - Stylized Low Poly 3D Art : https://assetstore.unity.com/packages/essentials/tutorial-projects/starter-pack-synty-polygon-stylized-low-poly-3d-art-156819

Test user:
email : testuser2@gmail.com
user name : testuser2
password : testuser2345

//Azure Function
        [Function("UpdateGameLaunch")]
        public async Task<IActionResult> UpdateGameLaunch(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Get the partition key value from the query string parameter "partitionKey"
            string partitionKey = req.Query["partitionKey"];

            // Check if partition key is provided
            if (string.IsNullOrEmpty(partitionKey))
            {
                return new BadRequestObjectResult("PartitionKey parameter is required.");
            }

            // Row key olarak partition key değerini kullanıyoruz
            string rowKey = partitionKey;

            Response<TableEntity>? entity = null;
            // Retrieve the entity
            try
            {
                entity = await _tableClient.GetEntityAsync<TableEntity>(partitionKey, rowKey);
            }
            catch (RequestFailedException ex)
            {
                _logger.LogWarning($"Entity not found or error retrieving entity: {ex.Message}");
            }
            

            if (entity != null)
            {
                // Increment the value
                int currentValue = (int)entity.Value["GameLaunch"];
                entity.Value["GameLaunch"] = currentValue + 1;

                // Update the table
                await _tableClient.UpdateEntityAsync(entity.Value, ETag.All, TableUpdateMode.Replace);

                return new OkObjectResult(entity.Value["GameLaunch"]);
            }
            else
            {
                // Eğer entity bulunamazsa, yeni bir entity oluşturun ve GameLaunch değerini 1 olarak ayarlayın
                var newEntity = new TableEntity(partitionKey, rowKey)
                {
                    { "GameLaunch", 1 }
                };
                await _tableClient.AddEntityAsync(newEntity);

                return new OkObjectResult(1);
            }
        }

//PlayFab API
handlers.updateGameLaunch = function (args, context) {
    // The pre-defined "currentPlayerId" variable is initialized to the PlayFab ID of the player logged-in on the game client. 
    var playFabId = currentPlayerId;

    // Construct the URL with the currentPlayerId as partitionKey
    var url = "https://mgp.azurewebsites.net/api/UpdateGameLaunch?code=ocrXCUU_nWGpQWRlTJ4cNikboIw04OIPFz4q7tQoUVpvAzFuwG2bjw%3D%3D&partitionKey=" + playFabId;
    
    // Send the HTTP GET request
    var response = http.request(url, "get");

    // Log the response from the request
    log.info("HTTP request sent to update game launch for player: " + playFabId);
    log.info("Response from server: " + JSON.stringify(response));

    // Return the response to the client
    return { responseContent: response };
};


Editor Class Example

#if UNITY_EDITOR
[CustomEditor(typeof(RaycastTargetCounter))]
public class RaycastTargetCounterEditor : Editor
{
    RaycastTargetCounter script;

    private void OnEnable()
    {
        script = (RaycastTargetCounter)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10);

        if (GUILayout.Button("Calculate All Have Raycast Target Object"))
        {
            script.RaycastCounter();
        }     
    }
}
#endif

!!! Editor Hatası !!!
Eğer her derlemeden sonra sıklıkla motor Reload Script Assemblies (Hold on ...) pop up ında sıkışıyorsa
çözüm;

Deleting the UserSettings/Layouts/default-2021.dwlt file 
This can also happen when there's an invalid link junction (one that points to a non-existing folder).