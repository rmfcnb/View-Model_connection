using System;
using System.Collections.Generic;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            View v = new View();
            v.Add("5");
            v.Remove("5");
        }
    }
    
    
    public delegate void Callback(int index);
    
    // ######################
    public class ViewNode{
        private string str;
        public ViewNode(string inStr){
            str = inStr;
        }
        
        public string GetValue(){
            return str;
        }
    } 
    // ######################
    public class ModelNode{
        private int num;
        
        private int ind;
        
        private Callback callback;
        
        public ModelNode(int inNum, Callback f, int index){
            num = inNum;
            callback = new Callback(f);
            ind = index;
        }
        
        public void Destroy(){
            callback(ind);
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
            list.Add(vn);
            
            model.Add(n, Destroy, list.Count-1);
        }
        
        public void Remove(string s){
        
            int n;
            bool l = Int32.TryParse(s,out n);
            if(!l) return;
            
            model.Remove(n);
        
        }
        
        public void Destroy(int n){
            Console.WriteLine("destroy: {0}", list[n].GetValue());
            list.RemoveAt(n);
        }
    }
    // ######################
    public class Model{
        private List<ModelNode> list;
        
        public Model(){
            list = new List<ModelNode>();
        }
        
        public void Add(int n, Callback c, int index){
            ModelNode mn = new ModelNode(n, c, index);
            list.Add(mn);
        }
        
        public void Remove(int n){
            int i = GetIndex(n);
            if(i != -1){
                list[i].Destroy();
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
    }
}
