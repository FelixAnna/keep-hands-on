namespace MovieStudio.Thirdparty
{
    public abstract class StudioEmployee
    {
        public long Salary { get; }
        public long EarnedMoney { get; private set; }
        public string Name { get; }

        public StudioEmployee(string name) { }

        public StudioEmployee(string name, long initialSalary)
        {
            this.Name = name;
            this.Salary = initialSalary;
            this.EarnedMoney = 0L;
        }

        public void PaySalary(long paidSum)
        {
            this.EarnedMoney += paidSum;
        }
    }
}
