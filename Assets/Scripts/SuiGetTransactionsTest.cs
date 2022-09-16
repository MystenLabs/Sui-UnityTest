using System;
using System.Threading.Tasks;
using SuiDotNet.Client;
using SuiDotNet.Client.Requests;
using UnityEngine;

[RequireComponent(typeof(SuiClient))]
public class SuiGetTransactionsTest : MonoBehaviour
{
    public SuiJsonClient Client;
    public Task<SuiTransactionResponse> GetTransactionTask;
    public Task<SequencedTransaction[]> GetTransactionsForAddressTask;
    public Task<SequencedTransaction[]> GetTransactionsForObjectTask;

    // substitute your preferred Sui node endpoint here, either in code or in Unity inspector
    public String SuiNodeUrl = "http://127.0.0.1:9000";
    // substitute your transaction digest here
    public String TestTransactionDigest = "XbyTWT8UZ1qXEM4/sdRLOzF+uwtHEEjuoj8m0VKVZFQ=";
    public String TestAddress = "0x08c9e31048ce86a3538272d4adaf2069ecf26c01";
    public String TestObject = "0x0754d12ea8d4698cb861d11cb192574b53c19925";

    void Start()
    {
        var settings = new SuiClientSettings() { BaseUri = SuiNodeUrl };
        Client = new SuiJsonClient(settings);
    
        GetTransactionTask = Client.GetTransactionWithEffects(TestTransactionDigest);
        GetTransactionsForAddressTask = Client.GetTransactionsForAddress(TestAddress);
        GetTransactionsForObjectTask = Client.GetTransactionsForObject(TestObject);
    }

    void Update()
    {
        if (GetTransactionTask is {IsCompleted: true})
        {
            var result = GetTransactionTask.Result;
            Debug.Log("get tx - cert, digest: " + result.Certificate.TransactionDigest);
            Debug.Log("get tx - cert, signature: " + result.Certificate.Signature);
            Debug.Log("get tx - cert, data: " + result.Certificate.Data);

            Debug.Log("get tx - cert, authSignInfo:\n" + result.Certificate.AuthoritySignInfo);
            
            Debug.Log("get transaction, time: " + result.Timestamp);
            Debug.Log("get transaction, effects: " + JsonUtility.ToJson(result.Effects));
            Debug.Log("get transaction, parsed: " + result.ParsedData);
            GetTransactionTask = null;
        }
        if (GetTransactionsForAddressTask is {IsCompleted: true})
        {
            Debug.Log("transactions for address count: " + GetTransactionsForAddressTask.Result.Length);
            foreach (var addrTx in GetTransactionsForAddressTask.Result)
                Debug.Log("get transactions for address result: [" + addrTx.SequenceNumber + ", " + addrTx.Digest + "]");
            GetTransactionsForAddressTask = null;
        }
        if (GetTransactionsForObjectTask is {IsCompleted: true})
        {
            Debug.Log("transactions for object count: " + GetTransactionsForObjectTask.Result.Length);
            foreach (var objTx in GetTransactionsForObjectTask.Result)
                Debug.Log("get transactions for object result: [" + objTx.SequenceNumber + ", " + objTx.Digest + "]");
            GetTransactionsForObjectTask = null;
        }
    }
}