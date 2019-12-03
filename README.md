#### request to googleAutoML 

curl -H "Content-Type: text/plain" -H "Authorization: Bearer ya29-----31QV" --data "@googelautoml_request_data.json"  https://automl.googleapis.com/v1beta1/projects/safemailautoml/locations/us-central1/models/TCN6737749096603608115:predict

 
##### postbody data file  googelautoml_request_data.json
```
{
  "payload": {
    "textSnippet": {
      "content": "test<!--Title Content Separator-->I am going to kill myself.\\r\\n",
      "mime_type": "text/plain"
    }
  }
}
```

response: 
```
{
  "payload": [
    {
      "classification": {
        "score": 0.82916576
      },
      "displayName": "True_Positive"
    },
    {
      "classification": {
        "score": 0.17083418
      },
      "displayName": "False_Positives"
    }
  ]
}

```
