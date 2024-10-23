public class Solution
{
    public int FindCheapestPrice(int n, int[][] flights, int src, int dst, int k)
    {
        var a = GetGraph(flights, src, out Dictionary<int,Node> nodes);
        SetPrices(a, 0, 0, src,k);

        var ds = nodes[dst];
        var bestPrice = ds.HopsAndPrices.Where(x => x.Key <= k).Select(x => x.Value).OrderBy(x=>x).FirstOrDefault();
        
        return bestPrice == default(int) ? -1 : bestPrice;
    }

    public Node GetGraph(int[][] flights, int src, out Dictionary<int,Node> nodes)
    {
        nodes = new Dictionary<int, Node>();

        foreach (var flight in flights)
        {
            nodes.TryGetValue(flight[0], out var srcNode);

            if (srcNode == null)
            {
                srcNode = new Node() { NodeId = flight[0] };
                nodes.Add(flight[0], srcNode);
            }

            var destination = flight[1];
            var price = flight[2];

            nodes.TryGetValue(destination, out var destNode);

            if (destNode == null)
            {
                destNode = new Node() { NodeId = destination };
                nodes.Add(destination, destNode);

            }

            var newLink = new Link()
            {
                NodeFrom = srcNode,
                NodeTo = destNode,
                Price = price
            };

            srcNode.Children.Add(newLink);
        }

        return nodes[src];

    }

    public void SetPrices(Node node, int stop, int priceToGet, int from, int maxStops)
    {
        if (stop > maxStops)
        {
            return;
        }
        
        foreach (var link in node.Children)
        {
            if (link.NodeTo.CheckedBy.ContainsKey(node.NodeId))
            {
                if (link.NodeTo.CheckedBy[node.NodeId] < link.Price + priceToGet)
                {
                    continue;
                }
            }
            
            if (!link.NodeTo.HopsAndPrices.TryGetValue(stop, out var price))
            {
                link.NodeTo.HopsAndPrices.Add(stop, link.Price + priceToGet);
            }
            else if(price > priceToGet + link.Price)
            {
                link.NodeTo.HopsAndPrices[stop] = priceToGet + link.Price;
            }
        }
        
        if (node.CheckedBy.ContainsKey(from))
        {
            node.CheckedBy[from] = priceToGet;
        }
        else
        {
            node.CheckedBy.Add(from, priceToGet);
        }

    

        foreach (var link in node.Children)
        {
            if (link.NodeTo.CheckedBy.TryGetValue(node.NodeId, out var price))
            {
                if (price < priceToGet + link.Price)
                {
                    continue;
                }
            }
            
            SetPrices(link.NodeTo, stop+1, link.Price+priceToGet, node.NodeId,maxStops);
        }

    }
    
    
    public class Node
    {
        public Node()
        {
            HopsAndPrices = new Dictionary<int, int>();
            Children = new List<Link>();
            Parent = new List<Link>();
            CheckedBy = new Dictionary<int, int>();
        }

        public Dictionary<int, int> CheckedBy { get; set; }

        public int NodeId { get; set; }
        
        public Dictionary<int, int> HopsAndPrices { get; }
        
        public List<Link> Children { get; set; } 
        
        public List<Link> Parent { get; set; }

        public override string ToString()
        {
            return $"{NodeId}";
        }
    }

    public class Link
    {
        public Link()
        {
        }

        public Node NodeFrom { get; set; }
        
        public Node NodeTo { get; set; }
        
        public int Price { get; set; }

        public override string ToString()
        {
            return $"From: {NodeFrom.NodeId} To: {NodeTo.NodeId} Price: {Price}";
        }
    }
}