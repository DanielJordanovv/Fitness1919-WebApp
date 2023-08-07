using Fitness1919.Web.ViewModels.CalorieCalculator;
using Microsoft.AspNetCore.Mvc;
using static Fitness1919.Common.NotificationMessagesConstants;

namespace Fitness1919.Web.Controllers
{
    public class CalorieCalculatorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new CalorieCalculatorViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculate([Bind("Gender, Age, Height, Weight, WeeklyTrainingDays, Goal")] CalorieCalculatorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            double bmr = Math.Ceiling(CalculateBMR(model.Gender, model.Age, model.Height, model.Weight));
            double tdee = Math.Ceiling(CalculateTDEE(bmr, model.WeeklyTrainingDays));
            double targetCalories = Math.Ceiling(CalculateTargetCalories(tdee, model.Goal));

            ViewBag.CalorieIntake = $"Your daily calorie intake should be approximately: {targetCalories} calories.";

            return PartialView("CalorieResult");
        }

        private double CalculateBMR(string gender, int age, double height, double weight)
        {
            double bmr = 0;
            if (gender == "male")
            {
                bmr = 88.362 + (13.397 * weight) + (4.799 * height) - (5.677 * age);
            }
            else if (gender == "female")
            {
                bmr = 447.593 + (9.247 * weight) + (3.098 * height) - (4.330 * age);
            }
            return bmr;
        }

        private double CalculateTDEE(double bmr, int weeklyTrainingDays)
        {
            double tdee = 0;
            double activityFactor = 1.2;
            if (weeklyTrainingDays >= 3 && weeklyTrainingDays <= 5)
            {
                activityFactor = 1.55;
            }
            else if (weeklyTrainingDays >= 6 && weeklyTrainingDays <= 7)
            {
                activityFactor = 1.9;
            }

            tdee = bmr * activityFactor;
            return tdee;
        }

        private double CalculateTargetCalories(double tdee, string goal)
        {
            double targetCalories = 0;
            switch (goal)
            {
                case "gain":
                    targetCalories = tdee + 500;
                    break;
                case "lose":
                    targetCalories = tdee - 500;
                    break;
                case "maintain":
                    targetCalories = tdee;
                    break;
                default:
                    targetCalories = tdee;
                    break;
            }
            return targetCalories;
        }
    }
}
