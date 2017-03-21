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
            v.Flash("5");
            v.Remove("5");
        }
    }
    
    // ######################
    public class ViewNode{
        private string str;
        
        private event Action<ViewNode> Destroy;
        
        public ViewNode(string inStr, Action<ViewNode> destroy){
            str = inStr;
            Destroy += destroy;
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
        
        public event Action Flash;
        
        public event Action Destroy;
        
        public ModelNode(int inNum, Action flash, Action destroy, int index){
            Flash += flash;
            Destroy += destroy;
            num = inNum;
        }
        
        public void DestroyObject(){
            Destroy.Invoke();
        }
        
        public void FlashObject(){
            Flash.Invoke();
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
            
            ViewNode vn = new ViewNode(s,Destroy);
            list.Add(vn);
            
            model.Add(n, vn.FlashObject, vn.DestroyObject, list.Count-1);
        }
        
        public void Remove(string s){
        
            int n;
            bool l = Int32.TryParse(s,out n);
            if(!l) return;
            
            model.Remove(n);
        
        }
        
        public void Flash(string s){
            
            int n;
            bool l = Int32.TryParse(s,out n);
            if(!l) return;
            
            model.Flash(n);
        }
        
        private void Destroy(object sender){
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
        
        public void Add(int n, Action flash, Action destroy, int index){
            ModelNode mn = new ModelNode(n, flash, destroy, index);
            list.Add(mn);
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
        
        public void Flash(int n){
        
            foreach(ModelNode mn in list){
                if(mn.GetValue() == n){
                    mn.FlashObject();
                    break;
                }
            }
        }
    }
}
