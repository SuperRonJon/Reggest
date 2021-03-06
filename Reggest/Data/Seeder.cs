﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reggest.Components.fitness;
using Reggest.Components.qAndA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reggest.Data
{
    public static class Seeder
    {
        const string GZCLP = "GZCLP";
        const string StartingStrength = "Starting Strength";
        const string StrongLifts = "StrongLifts 5x5";
        const string StrongCurves = "Strong Curves";
        const string ThinnerLeanerStronger = "Thinner Leaner Stronger";
        const string DumbbellOnlyBasics = "Dumbbell Only Basics";
        const string TwoSuns = "2_Suns' 5/3/1";

        public static List<FitnessPlan> FitnessPlans { get; set; }
        public static List<Question> Questions { get; set; }
        private static IAnswerService _answerService;
        private static IQuestionService _questionService;
        private static IFitnessPlanService _fitnessPlanService;

        public static async Task<IWebHost> SeedData(this IWebHost webHost)
        {
            using (var scope = webHost.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    _answerService = scope.ServiceProvider.GetRequiredService<IAnswerService>();
                    _questionService = scope.ServiceProvider.GetRequiredService<IQuestionService>();
                    _fitnessPlanService = scope.ServiceProvider.GetRequiredService<IFitnessPlanService>();

                    context.Database.Migrate();

                    SeedFitnessPlans();
                    SeedQandA();
                }
            }

            return webHost;
        }

        private static void SeedQandA()
        {
            List<Answer> GetAnswers()
            {
                var answers = new List<Answer>
                {
                    new Answer { AnswerText = "A lot", Points = (int)AnswerMatch.FirstAnswer },
                    new Answer { AnswerText = "Some", Points = (int)AnswerMatch.SecondAnswer },
                    new Answer { AnswerText = "Not much", Points = (int)AnswerMatch.ThirdAnswer },
                    new Answer { AnswerText = "Not at all", Points = (int)AnswerMatch.FourthAnswer },
                };

                return answers;
            }

            List<Answer> GetAnswersForImportance()
            {
                var answers = new List<Answer>
                {
                    new Answer { AnswerText = "Very important", Points = (int)AnswerMatch.FirstAnswer },
                    new Answer { AnswerText = "Somewhat important", Points = (int)AnswerMatch.SecondAnswer },
                    new Answer { AnswerText = "Not that important", Points = (int)AnswerMatch.ThirdAnswer },
                    new Answer { AnswerText = "Not a goal at all", Points = (int)AnswerMatch.FourthAnswer },
                };

                return answers;
            }

            int GetFitnessPlanId(string name)
            {
                return _fitnessPlanService.GetAll().Single(x => x.Name == name).Id;
            }

            int GetVariationPlanId(string name, int days)
            {
                return _fitnessPlanService.GetAll().Single(x => x.Name == name).VariationPlans.Single(x => x.DaysPerWeek == days).Id;
            }

            Questions = new List<Question>
            {
                new Question
                {
                    QuestionText = "How long do you want your workout to be?",
                    FitnessPlanAnswerPoints = new List<FitnessPlanAnswerPoint>
                    {
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(GZCLP), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StartingStrength), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongLifts), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongCurves), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(StrongCurves, 4), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(ThinnerLeanerStronger), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 4), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 5), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(DumbbellOnlyBasics), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(TwoSuns), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 5), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 6), Points = (int)AnswerMatch.FourthAnswer },
                    },
                    Answers = new List<Answer>
                    {
                        new Answer { AnswerText = "30 minutes - 1 hour", Points = (int)AnswerMatch.FirstAnswer },
                        new Answer { AnswerText = "1 hour - 1.5 hours", Points = (int)AnswerMatch.SecondAnswer },
                        new Answer { AnswerText = "1.5 hours - 2 hours", Points = (int)AnswerMatch.ThirdAnswer },
                        new Answer { AnswerText = "2+ hours", Points = (int)AnswerMatch.FourthAnswer },
                    }
                },
                new Question
                {
                    QuestionText = "How many days a week do you want to workout?",
                    FitnessPlanAnswerPoints = new List<FitnessPlanAnswerPoint>
                    {
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(GZCLP), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StartingStrength), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongLifts), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongCurves), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(StrongCurves, 4), Points = (int)AnswerMatch.FourthAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(ThinnerLeanerStronger), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 4), Points = (int)AnswerMatch.FourthAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 5), Points = (int)AnswerMatch.FifthAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(DumbbellOnlyBasics), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(TwoSuns), Points = (int)AnswerMatch.FourthAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 5), Points = (int)AnswerMatch.FifthAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 6), Points = (int)AnswerMatch.SixthAnswer },
                    },
                    Answers = new List<Answer>
                    {
                        new Answer { AnswerText = "0-1", Points = (int)AnswerMatch.FirstAnswer },
                        new Answer { AnswerText = "2", Points = (int)AnswerMatch.SecondAnswer },
                        new Answer { AnswerText = "3", Points = (int)AnswerMatch.ThirdAnswer },
                        new Answer { AnswerText = "4", Points = (int)AnswerMatch.FourthAnswer },
                        new Answer { AnswerText = "5", Points = (int)AnswerMatch.FifthAnswer },
                        new Answer { AnswerText = "6-7", Points = (int)AnswerMatch.SixthAnswer },
                    }
                },
                new Question
                {
                    QuestionText = "How much do you want to use dumbbells in your routine?",
                    FitnessPlanAnswerPoints = new List<FitnessPlanAnswerPoint>
                    {
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(GZCLP), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StartingStrength), Points = (int)AnswerMatch.FourthAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongLifts), Points = (int)AnswerMatch.FourthAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongCurves), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(StrongCurves, 4), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(ThinnerLeanerStronger), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 4), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 5), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(DumbbellOnlyBasics), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(TwoSuns), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 5), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 6), Points = (int)AnswerMatch.ThirdAnswer },
                    },
                    Answers = GetAnswers(),
                },
                new Question
                {
                    QuestionText = "How much do you want to use barbells in your routine?",
                    FitnessPlanAnswerPoints = new List<FitnessPlanAnswerPoint>
                    {
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(GZCLP), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StartingStrength), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongLifts), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongCurves), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(StrongCurves, 4), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(ThinnerLeanerStronger), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 4), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 5), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(DumbbellOnlyBasics), Points = (int)AnswerMatch.FourthAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(TwoSuns), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 5), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 6), Points = (int)AnswerMatch.FirstAnswer },
                    },
                    Answers = GetAnswers(),
                },
                new Question
                {
                    QuestionText = "How important is increasing aesthetics as a goal for you?",
                    FitnessPlanAnswerPoints = new List<FitnessPlanAnswerPoint>
                    {
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(GZCLP), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StartingStrength), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongLifts), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongCurves), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(StrongCurves, 4), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(ThinnerLeanerStronger), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 4), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 5), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(DumbbellOnlyBasics), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(TwoSuns), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 5), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 6), Points = (int)AnswerMatch.SecondAnswer },
                    },
                    Answers = GetAnswersForImportance(),
                },
                new Question
                {
                    QuestionText = "How important is gaining strength as a goal for you?",
                    FitnessPlanAnswerPoints = new List<FitnessPlanAnswerPoint>
                    {
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(GZCLP), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StartingStrength), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongLifts), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongCurves), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(StrongCurves, 4), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(ThinnerLeanerStronger), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 4), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 5), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(DumbbellOnlyBasics), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(TwoSuns), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 5), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 6), Points = (int)AnswerMatch.FirstAnswer },
                    },
                    Answers = GetAnswersForImportance(),
                },
                new Question
                {
                    QuestionText = "How important is lower body development (glutes, legs etc.) to you?",
                    FitnessPlanAnswerPoints = new List<FitnessPlanAnswerPoint>
                    {
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(GZCLP), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StartingStrength), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongLifts), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongCurves), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(StrongCurves, 4), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(ThinnerLeanerStronger), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 4), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 5), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(DumbbellOnlyBasics), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(TwoSuns), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 5), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 6), Points = (int)AnswerMatch.FirstAnswer },
                    },
                    Answers = GetAnswersForImportance(),
                },
                                new Question
                {
                    QuestionText = "How important is upper body development (arms, chest, shoulders etc.) to you?",
                    FitnessPlanAnswerPoints = new List<FitnessPlanAnswerPoint>
                    {
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(GZCLP), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StartingStrength), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongLifts), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(StrongCurves), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(StrongCurves, 4), Points = (int)AnswerMatch.ThirdAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(ThinnerLeanerStronger), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 4), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(ThinnerLeanerStronger, 5), Points = (int)AnswerMatch.FirstAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(DumbbellOnlyBasics), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetFitnessPlanId(TwoSuns), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 5), Points = (int)AnswerMatch.SecondAnswer },
                        new FitnessPlanAnswerPoint { FitnessPlanId = GetVariationPlanId(TwoSuns, 6), Points = (int)AnswerMatch.SecondAnswer },
                    },
                    Answers = GetAnswersForImportance(),
                },
            };

            var questions = _questionService.GetAll();

            _questionService.AddQuestions(Questions.Where(question => !questions.Any(x => x.QuestionText == question.QuestionText)).ToList());
        }

        private static void SeedFitnessPlans()
        {
            FitnessPlans = new List<FitnessPlan>
            {
                new FitnessPlan
                {
                    Name = GZCLP,
                    Description = "Free novice strength training program with linear progression. Focuses on main barbell movements with machine and dumbbell accessories, making it easy to customize for aesthetic goals. Many prefer the structure and body part balance of GZCL over StrongLifts and Starting Strength, but there is no movement instruction provided. 3 days per week.",
                    Link = "https://www.reddit.com/r/Fitness/comments/6pjiwd/heres_a_quick_summary_of_the_gzclp_linear/",
                    DaysPerWeek = 3,
                    TimeToCompleteWorkout = TimeSpan.FromHours(1)
                },
                new FitnessPlan
                {
                    Name = StartingStrength,
                    Description = "Book paired with a simple program for foundational strength. 3 days/week workout schedule on only the major barbell movements (no accessory work).",
                    Link = "https://startingstrength.com/",
                    DaysPerWeek = 3,
                    TimeToCompleteWorkout = TimeSpan.FromHours(1)
                },
                new FitnessPlan
                {
                    Name = StrongLifts,
                    Description = "Free website-based beginner's program. 3 days/week workout schedule on only the major barbell movements (no accessory work). Very similar to Starting Strength but adds rows and adjusts the volume slightly.",
                    Link = "https://stronglifts.com/5x5/",
                    DaysPerWeek = 3,
                    TimeToCompleteWorkout = TimeSpan.FromHours(1)
                },
                new FitnessPlan
                {
                    Name = StrongCurves,
                    Description = "An aesthetics-based beginners program. Focuses on glute and lower body development, while also including upper body work as well.",
                    Link = "https://www.reddit.com/r/xxfitness/comments/3k7e47/ok_the_color_coded_google_excel_strong_curve/?st=jfzvxru9&sh=a1dd43f2",
                    DaysPerWeek = 3,
                    TimeToCompleteWorkout = TimeSpan.FromHours(1),
                    VariationPlans = new List<FitnessPlan> {
                        new FitnessPlan { DaysPerWeek = 4 }
                    }
                },
                new FitnessPlan
                {
                    Name = ThinnerLeanerStronger,
                    Description = "Book with fitness program, diet advice, supplement recommendations, etc. Comes in 3/4/5 day variants.",
                    Link = "https://www.muscleforlife.com/thinner-leaner-stronger/",
                    DaysPerWeek = 3,
                    TimeToCompleteWorkout = TimeSpan.FromHours(1),
                    VariationPlans = new List<FitnessPlan> {
                        new FitnessPlan { DaysPerWeek = 4 },
                        new FitnessPlan { DaysPerWeek = 5 }
                    }
                },
                new FitnessPlan
                {
                    Name = DumbbellOnlyBasics,
                    Description = "For those limited to dumbbells only or who want to start with less technical movements. A traditional bodypart 'split' routine with days dedicated to chest/triceps, back/biceps, and legs/shoulders. 3 days per week.",
                    Link = "https://www.muscleandstrength.com/workouts/frankoman-dumbbell-only-split.html",
                    TimeToCompleteWorkout = TimeSpan.FromMinutes(45),
                    DaysPerWeek = 3,
                },
                new FitnessPlan
                {
                    Name = TwoSuns,
                    Description = "The nSuns (2suns) Linear Progression program is that takes inspiration from a few other popular programs and condenses it down into a single high volume week. Involves heavy amounts of squats/deadlifts/benching.",
                    DaysPerWeek = 4,
                    TimeToCompleteWorkout = TimeSpan.FromHours(1.5),
                    VariationPlans = new List<FitnessPlan> {
                        new FitnessPlan { DaysPerWeek = 5 },
                        new FitnessPlan { DaysPerWeek = 6 }
                    }
                }
            };

            foreach (var fitnessPlan in FitnessPlans)
            {
                foreach (var fitnessPlanVariation in fitnessPlan.VariationPlans)
                {
                    fitnessPlanVariation.ParentFitnessPlan = fitnessPlan;
                }
            }

            var fitnessPlans = _fitnessPlanService.GetAll();

            _fitnessPlanService.AddFitnessPlans(FitnessPlans.Where(fitnessPlan => !fitnessPlans.Any(x => x.Name == fitnessPlan.Name)).ToList());
        }
    }
}
