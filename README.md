#### request to googleAutoML 

###### get google access token

```
GOOGLE_APPLICATION_CREDENTIALS=C:\safemail\config\GOOGLE_APPLICATION_CREDENTIALS.json
gcloud auth application-default print-access-token
```
curl -X POST \
  -H "Authorization: Bearer $(gcloud auth application-default print-access-token)" \
  -H "Content-Type: application/json" \
  https://automl.googleapis.com/v1beta1/projects/873648580581/locations/us-central1/models/TCN2177783489896119268:predict \
  -d @request.json
 ```
 1  set token= {respone-of-previous-cmd}
 FOR /F "tokens=*" %g IN ('gcloud auth application-default print-access-token') do (SET token=%g)
2 check token 
echo %token%
3 create data file  googelautoml_request_data.json
 
{
  "payload": {
    "textSnippet": {
      "content": "test<!--Title Content Separator-->I am going to kill myself.\\r\\n",
      "mime_type": "text/plain"
    }
  }
}
 
4 do prediction 
curl -H "Content-Type: application/json" -H "Authorization: %token%" --data "@googelautoml_request_data.json"  https://automl.googleapis.com/v1beta1/projects/safemailautoml/locations/us-central1/models/TCN6737749096603608115:predict
 ```
5 response: 
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
