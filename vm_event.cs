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
        private string displayString;
        
        public event Action<ViewNode> Destroyed;
        
        public ViewNode(ModelNode mn){
            displayString = mn.GetValue().ToString();
            mn.Destroyed += DestroyObject;
            mn.Found += FlashObject;
        }
        
        public string GetValue(){
            return displayString;
        }
        
        public void FlashObject(){
            Console.WriteLine("Node with value {0} is flashing!",displayString);
        }
        
        public void DestroyObject(){
            Destroyed(this);
        }
    } 
    // ######################
    public class ModelNode : IDisposable{
        private int num;
        
        public event Action Found;
        
        public event Action Destroyed;
        
        public ModelNode(int inNum){
            num = inNum;
        }
        
        public void Dispose(){
            Destroyed.Invoke();
        }
        
        public void FoundObject(){
            Found.Invoke();
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
        
        public View Add(string s){
            int n;
            bool l = Int32.TryParse(s, out n);
            
            if(!l) return null;
            
            ModelNode mn = model.Add(n);
            
            ViewNode vn = new ViewNode(mn);
            vn.Destroyed += DestroyObject;
            list.Add(vn);
            
            return this;
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
        
        public ModelNode Add(int n){
            ModelNode mn = new ModelNode(n);
            list.Add(mn);
            return mn;
        }
        
        public void Remove(int n){
            ModelNode mn = list.Find(x => x.GetValue() == n);
            if(mn != null){
                list.Remove(mn);
                mn.Dispose();
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
                    mn.FoundObject();
                    break;
                }
            }
        }
    }
}
