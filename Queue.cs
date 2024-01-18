namespace HandleMessage;

public class Queue
{
    private List<string> data = new List<string>();

    public void Enqueue(string message)
    {
        data.Add(message);
    }

    public string Dequeue()
    {
        if (data.Count > 0)
        {
            var dataReturn = this.data[0];
            data.RemoveAt(0);
            return dataReturn;
        }
        else
        {
            return null;
        }
    }

    public string Peek()
    {
        if (data.Count > 0)
        {
            return data[0];
        }
        else
        {
            // throw new InvalidOperationException("Queue is empty.");
            return string.Empty;
        }
    }

    public int Size()
    {
        return data.Count();
    }
}