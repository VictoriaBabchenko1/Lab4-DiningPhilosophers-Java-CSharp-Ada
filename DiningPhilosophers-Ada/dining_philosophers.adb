with Ada.Text_IO, Ada.Integer_Text_IO;
use Ada.Text_IO, Ada.Integer_Text_IO;

procedure Dining_Philosophers is

   type Fork_Access is access all Boolean;
   Forks : array (0 .. 4) of aliased Boolean := (others => True);

   task type Philosopher(Id : Integer; Left, Right : Fork_Access);

   task body Philosopher is
   begin
      for I in 1 .. 10 loop
         Put_Line("Philosopher" & Integer'Image(Id) & " is thinking.");
         if Id = 4 then
            while not Right.all loop null; end loop;
            Right.all := False;
            while not Left.all loop null; end loop;
            Left.all := False;
         else
            while not Left.all loop null; end loop;
            Left.all := False;
            while not Right.all loop null; end loop;
            Right.all := False;
         end if;
         Put_Line("Philosopher" & Integer'Image(Id) & " is eating.");
         delay 0.1;
         Left.all := True;
         Right.all := True;
      end loop;
   end Philosopher;

   Phil_0 : Philosopher(0, Forks(0)'Access, Forks(1)'Access);
   Phil_1 : Philosopher(1, Forks(1)'Access, Forks(2)'Access);
   Phil_2 : Philosopher(2, Forks(2)'Access, Forks(3)'Access);
   Phil_3 : Philosopher(3, Forks(3)'Access, Forks(4)'Access);
   Phil_4 : Philosopher(4, Forks(4)'Access, Forks(0)'Access);

begin
   delay 5.0;
end Dining_Philosophers;
