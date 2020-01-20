using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class TaskHolderTest : MonoBehaviour
{
    public List<AsynTaskClassA> tasksClass = new List<AsynTaskClassA>();

    void Awake()
    {
        ActivateTasks();
    }

    private void ActivateTasks()
    {

        List<Task> tasks = new List<Task>();
        foreach (var t in tasksClass)
        {
            var task = t.DoAsTask();
            tasks.Add(task);

        }

        Debug.Log("START EXECUTE");
        var taskArray = tasks.ToArray();
        Task.WaitAll(taskArray);
        Debug.Log("ALL COMPLETED");
    }
}
