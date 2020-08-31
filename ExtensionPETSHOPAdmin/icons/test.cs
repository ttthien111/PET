public void int fun1(int x, int y)  
{ 
  if(x == 0) 
    return y; 
  else
    return fun1(x - 1,  x + y); 
} 
public void main(){
    func1(1,2);
    
}