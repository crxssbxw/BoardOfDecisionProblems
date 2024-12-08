﻿using BoardOfDecisionProblems.Commands;
using BoardOfDecisionProblems.Forms;
using BoardOfDecisionProblems.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardOfDecisionProblems.ViewModel
{
    public class ResponsiblesViewModel : BaseViewModel
    {
        private ObservableCollection<Responsible> responsibles = new();
        public ObservableCollection<Responsible> Responsibles
        {
            get => responsibles;
            set
            {
                responsibles = value;
                OnPropertyChanged(nameof(Responsibles));
            }
        }

        private Responsible selectedResponsibility;
        public Responsible SelectedResponsibility
        {
            get => selectedResponsibility;
            set
            {
                selectedResponsibility = value;
                OnPropertyChanged(nameof(SelectedResponsibility));
            }
        }

        public ResponsiblesViewModel()
        {
            dbContext.Workers.Load();
            dbContext.Departments.Load();
            foreach(var responsible in dbContext.Responsibles)
            {
                Responsibles.Add(responsible);
            }
            ViewSource.Source = Responsibles;
        }

        #region Commands

        /*
         Команда set должна отвечать за переназначение работников на участке, делая их ответственными или снимая с них эту ответственность.
        Если переназначается существующий работник, ему устанавливается флаг IsCurrent, обозначающий, что в текущий момент он ответственный на этом участке.
        При этом у предыдущего назначенного этот флаг снимается.
        Если добавляется новый ответственный, добавляется новая запись с ответственностью, устанавливается флаг IsCurrent, у предыдущего - снимается.
        Реализовано это для того, чтобы была возможность просматривать ответственных за все время.
         */

        private RelayCommand set;
        public RelayCommand Set
        {
            get
            {
                return set ?? (set = new(obj =>
                {
                    if(SelectedResponsibility != null)
                    {
                        //Автоподстановка выбранного отдела и работника в окно переназначения
                    }
                    else
                    {
                        Responsible responsible = new();
                        SetResponsibility set = new SetResponsibility();
                        set.DataContext = responsible;
                        set.WorkersBox.ItemsSource = ProblemViewModel.WorkersViewModel.Workers;
                        set.DepartmentsBox.ItemsSource = ProblemViewModel.DepartmentsViewModel.Departments;

                        if(set.ShowDialog() == true)
                        {
                            responsible.IsCurrent = true;

                            var prevResponsible = Responsibles.Where(a => a.Department == responsible.Department
                                && a.IsCurrent == true).FirstOrDefault();
                            if(prevResponsible != null)
                            { 
                                prevResponsible.IsCurrent = false;

                                var dbpr = dbContext.Responsibles.Where(a => a.Department == responsible.Department 
                                    && a.IsCurrent == true).FirstOrDefault();
                                var temp = dbpr;
                                temp.IsCurrent = false;
                                dbContext.Responsibles.Entry(dbpr).CurrentValues.SetValues(temp);
                                dbContext.SaveChanges();
                            }

                            if (Responsibles.Any(a => a.Worker == responsible.Worker))
                            {
                                var currentResponsible = Responsibles.Where(a => a.Worker == responsible.Worker).FirstOrDefault();
                                currentResponsible.IsCurrent = true;

                                var dbcr = dbContext.Responsibles.Where(a => a.Worker == responsible.Worker).FirstOrDefault();
                                var temp = dbcr;
                                temp.IsCurrent = true;
                                dbContext.Responsibles.Entry(dbcr).CurrentValues.SetValues(temp);
                                dbContext.SaveChanges();
                            }
                            else
                            {
                                dbContext.Responsibles.Add(responsible);
                                dbContext.SaveChanges();
                                Responsibles.Add(responsible);
                            }
                        }
                        CollectionView.Refresh();
                    }
                }, obj => true));
            }
        }

        #endregion
    }
}
