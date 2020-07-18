using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.Dto;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
    [Route("api/fornecedores")]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService; 
        private  readonly IMapper _mapper;

        public FornecedorController(IFornecedorRepository fornecedorRepository, IFornecedorService fornecedorService, IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;                                                                                                              x                                                                
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<FornecedorDto>> ObtemTodos()
        {
            var fornecedor = _mapper.Map<IEnumerable<FornecedorDto>>(await _fornecedorRepository.ObterTodos());
            return fornecedor;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorDto>> ObterPorId(Guid id)
        {
            var fornecedor = await ObterFornecedorProdutoEndereco(id);

            if (fornecedor == null)
                return NotFound();

            return fornecedor;
        }

        [HttpPost]
        public  async Task<ActionResult<FornecedorDto>> Adicionar(FornecedorDto fornecedorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDto);
            var result = await _fornecedorService.Adicionar(fornecedor);

            if (!result)
                return BadRequest();

            return Ok(fornecedor);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorDto>> Atualizar(Guid id, FornecedorDto fornecedorDto)
        {
            if (id != fornecedorDto.Id || !ModelState.IsValid)
                return BadRequest();


            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDto);
            var result = await _fornecedorService.Atualizar(fornecedor);

            if (!result)
                return BadRequest();

            return Ok(fornecedor);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorDto>> Excluir(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null)
                return NotFound();

            var result = await _fornecedorService.Remover(id);

            if (!result)
                return BadRequest();

            return Ok(fornecedor);
        }

        public async Task<FornecedorDto> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDto>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }

        public async Task<FornecedorDto> ObterFornecedorProdutoEndereco(Guid id) 
        {
            return _mapper.Map<FornecedorDto>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }
    }
}
