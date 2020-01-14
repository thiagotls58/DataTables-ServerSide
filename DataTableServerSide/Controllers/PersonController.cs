using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataTableServerSide.Data;
using DataTableServerSide.Models;
using System.Linq.Dynamic.Core;

namespace DataTableServerSide.Controllers
{
    public class PersonController : Controller
    {
        private readonly Context _context;

        public PersonController(Context context)
        {
            _context = context;
        }

        public IActionResult ListPersons()
        {
            return View();
        }

        public IActionResult SearchPersons(JQueryDataTablesParam Params)
        {
            //para informar ao datatables quantos registros existem AO TOTAL
            int TotalPessoas = 0;

            //para informar ao datatables quantos registros existem com o filtro aplicado
            int TotalPessoasFiltradas = 0;

            List<Person> Pessoas = this.SearchPersonsInDataBase(Params, out TotalPessoas, out TotalPessoasFiltradas);

            var Resultado = new
            {
                sEcho = Params.sEcho,
                iTotalRecords = TotalPessoas,
                iTotalDisplayRecords = TotalPessoasFiltradas,
                aaData = Pessoas
            };

            return Json(Resultado);
        }

        private List<Person> SearchPersonsInDataBase(JQueryDataTablesParam Params, out int TotalPessoas, out int TotalPessoasFiltradas)
        {
            //apenas pegando a referência aos dados que vamos lidar (não executa query nenhuma ainda)
            IQueryable<Person> Pessoas = _context.Person.AsQueryable();

            //se houver um filtro, vamos aplicá-lo (MAS AINDA SEM EXECUTAR A CONSULTA NO BANCO!)
            if (!string.IsNullOrWhiteSpace(Params.sSearch))
            {
                string search = Params.sSearch;

                bool isDate = false;
                DateTime dateValue = DateTime.Now;

                bool isInt = false;
                int intValue = 0;

                //é uma data?
                isDate = DateTime.TryParse(search, out dateValue);

                //é valor inteiro?
                isInt = int.TryParse(search, out intValue);

                //aplique o filtro em todas as colunas visíveis da tabela
                //eu optei por filtrar as colunas decimais e de data apenas quando o usuário inserir um valor compatível
                Pessoas = Pessoas.Where(x =>
                    (isInt && x.PersonID == intValue) ||
                    x.Name.Contains(search) ||
                    x.Email.Contains(search) ||
                    (isDate && x.DateOfBirth == dateValue) ||
                    x.PhoneNumber.Contains(search) ||
                    x.City.Contains(search)
                );
            }

            //fazemos um acesso ao banco para dizer quantas pessoas existem no DB
            TotalPessoas = _context.Person.Count();

            //mais um acesso para dizer quantas pessoas existem com os filtros aplicados
            TotalPessoasFiltradas = Pessoas.Count();

            //fazendo a ordenação pelo que foi informado na interface
            string ColunaOrdenada = Params.sColumns.Split(',')[Params.iSortCol_0];
            Pessoas = Pessoas.OrderBy(ColunaOrdenada + " " + Params.sSortDir_0);

            //pegando apenas os resultados relativos a página atual
            int itensTake = (Params.iDisplayLength == -1) ? TotalPessoas : Params.iDisplayLength;

            Pessoas = Pessoas.Skip(Params.iDisplayStart).Take(itensTake);

            //fazendo a query e retornando a viewmodel...
            return Pessoas.Select(x => new Person()
            {
                PersonID = x.PersonID,
                Name = x.Name,
                Email = x.Email,
                DateOfBirth = x.DateOfBirth,
                PhoneNumber = x.PhoneNumber,
                City = x.City,
            }).ToList();
        }

        // GET: Person
        public async Task<IActionResult> Index()
        {
            return View(await _context.Person.ToListAsync());
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonID == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonID,Name,Email,DateOfBirth,PhoneNumber,City")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonID,Name,Email,DateOfBirth,PhoneNumber,City")] Person person)
        {
            if (id != person.PersonID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonID == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Person.FindAsync(id);
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.PersonID == id);
        }
    }
}
