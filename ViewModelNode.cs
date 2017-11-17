using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyWest.Common.WPF
{
    public abstract class ViewModelNode
    {
        private ViewModelNode Root = null;
        private Dictionary<string, List<Action<object>>> Subscriptions = null;
        private List<ViewModelNode> Children = null;

        public Dictionary<string, object> Props = null;

        public ViewModelNode(ViewModelNode _root)
        {
            Root = _root;
            Props = new Dictionary<string, object>();
            Children = new List<ViewModelNode>();
            Subscriptions = new Dictionary<string, List<Action<object>>>();
        }

        public ViewModelNode GetRoot()
        {
            if (Root != null)
            {
                return Root;
            }

            return this;
        }

        public void AddProp(string property, object value)
        {
            Props.Add(property, value.Clone()); //value is default value
        }

        public ViewModelNode AddChild(ViewModelNode child)
        {
            Children.Add(child);
            return Children.LastOrDefault();
        }

        public List<ViewModelNode> GetChildren()
        {
            return Children;
        }

        public void Broadcast(string property, object data)
        {
            ViewModelNode root = GetRoot();

            if (root.Subscriptions.ContainsKey(property))
            {
                foreach (Action<object> action in root.Subscriptions[property])
                {
                    action(data);
                }
            }
        }

        public void Subscribe(string property, Action<object> handler)
        {
            ViewModelNode root = GetRoot(); //always at ultimate root node - so just provide direct reference somehow?, but could have clusters as well perhaps...

            if (root.Subscriptions.ContainsKey(property))
            {
                root.Subscriptions[property].Add(handler);
            } else
            {
                List<Action<object>> list = new List<Action<object>>();
                list.Add(handler);
                root.Subscriptions.Add(property, list);
            }
        }
    }
}
