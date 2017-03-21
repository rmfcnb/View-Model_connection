using System;
using System.Collections.Generic;

namespace Rextester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            View v = new View();
            v.Add("5");
            v.Search("5");
            v.Remove("5");
        }
    }
    
    // ######################
    public class ViewNode{
        private string str;
        
        public event Action<ViewNode> Destroy;
        
        public ViewNode(string inStr){
            str = inStr;
        }
        
        public string GetValue(){
            return str;
        }
        
        public void FlashObject(){
            Console.WriteLine("Node with value {0} is flashing!",str);
        }
        
        public void DestroyObject(){
            var d = Destroy;
            d(this);
        }
    } 
    // ######################
    public class ModelNode{
        private int num;
        
        public event Action Search;
        
        public event Action Destroy;
        
        public ModelNode(int inNum, int index){
            num = inNum;
        }
        
        public void DestroyObject(){
            Destroy.Invoke();
        }
        
        public void FlashObject(){
            Search.Invoke();
        }
        
        public int GetValue(){
            return num;
        }
    }
    // ######################
    public class View{
        private List<ViewNode> list;
        private Model model;
        
        public View(){
            list = new List<ViewNode>();
            model = new Model();
        }
        
        public void Add(string s){
            int n;
            bool l = Int32.TryParse(s, out n);
            
            if(!l) return;
            
            ViewNode vn = new ViewNode(s);
            vn.Destroy += DestroyObject;
            list.Add(vn);
            
            ModelNode mn = model.Add(n, list.Count-1);
            mn.Destroy += vn.DestroyObject;
            mn.Search += vn.FlashObject;
        }
        
        public void Remove(string s){
        
            int n;
            bool l = Int32.TryParse(s,out n);
            if(!l) return;
            
            model.Remove(n);
        
        }
        
        public void Search(string s){
            
            int n;
            bool l = Int32.TryParse(s,out n);
            if(!l) return;
            
            model.Search(n);
        }
        
        private void DestroyObject(object sender){
            ViewNode vn = sender as ViewNode;
            Console.WriteLine("destroy: {0}", vn.GetValue());
            list.Remove(vn);
        }
    }
    // ######################
    public class Model{
        private List<ModelNode> list;
        
        public Model(){
            list = new List<ModelNode>();
        }
        
        public ModelNode Add(int n, int index){
            ModelNode mn = new ModelNode(n, index);
            list.Add(mn);
            return mn;
        }
        
        public void Remove(int n){
            int i = GetIndex(n);
            if(i != -1){
                list[i].DestroyObject();
                list.RemoveAt(i);
            }
        }
        
        private int GetIndex(int n){
            for(int i = 0; i<list.Count; i++){
                if(list[i].GetValue() == n){
                    return i;
                }
            }
            return -1;
        }
        
        public void Search(int n){
        
            foreach(ModelNode mn in list){
                if(mn.GetValue() == n){
                    mn.FlashObject();
                    break;
                }
            }
        }
    }
}
