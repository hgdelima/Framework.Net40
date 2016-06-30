using Alcoa.Framework.Common.Presentation.Web;
using Microsoft.IdentityModel.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Alcoa.Framework.UnitTest.Common.Presentation
{
    [TestClass]
    public class BaseControllerTest
    {
        private ClaimCollection UserClaims;

        [TestInitialize]
        public void BaseControllerTestInitialize()
        {
            var claims = new List<Claim>
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier","SHPF"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name","SPF"),
                new Claim("http://schemas.alcoa.com/claims/app","SHPF"),
                new Claim("http://schemas.alcoa.com/claims/app/mnemonic","SHPF|SPF"),
                new Claim("http://schemas.alcoa.com/claims/app/profile","SHPF|1"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group","SHPF|1|Itens de Estoque"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/currentlevel","SHPF|1|Itens de Estoque|6"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/order","SHPF|1|Itens de Estoque|5"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/sublevels","SHPF|1|Itens de Estoque|8|7"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Envio de Itens para  o Certificado de Análise"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Envio de Itens para  o Certificado de Análise|/Producao/ItensEstoque/EnvioItensCertificado/EnvioItensCertificado.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group","SHPF|1|Inventário"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/currentlevel","SHPF|1|Inventário|8"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/order","SHPF|1|Inventário|6"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Inventário|Pesquisa de Inconsistência"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Inventário|Pesquisa de Inconsistência|/Producao/ItensEstoque/Inventario/PesquisaInconsistencia/InventarioPesquisaInconsistencia.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group","SHPF|1|Produção"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/currentlevel","SHPF|1|Produção|3"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/parentlevel","SHPF|1|Produção|0"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/order","SHPF|1|Produção|3"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/sublevels","SHPF|1|Produção|6|9|10|11|5"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Produção|Manutenção de Cadinhos Fábrica de Pó"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Produção|Manutenção de Cadinhos Fábrica de Pó|/SHPF/Producao/ManutencaoCadinhoFPO/ManutencaoCadinhoFPO.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Produção|Recebimento de Cadinhos Fábrica de Pó"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Produção|Recebimento de Cadinhos Fábrica de Pó|/Producao/RecebeCadinhoFPO/RecebeCadinhoFPO.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Produção|Consulta de Produção e Laboratório"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Produção|Consulta de Produção e Laboratório|/Laboratorio/AprovacaoAnalise/AprovacaoAnalise.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group","SHPF|1|Apoio"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/currentlevel","SHPF|1|Apoio|1"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/parentlevel","SHPF|1|Apoio|0"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/order","SHPF|1|Apoio|1"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Apoio|Manutenção de Warehouse x Usuários"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Apoio|Manutenção de Warehouse x Usuários|/Apoio/CadastroWarehouseUsuarios.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Manutenção de Itens"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Manutenção de Itens|/Producao/ItensEstoque/ManutencaoItensEstoque/PesquisaItensEstoque.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Registro de Produção Sow - Alumar"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Registro de Produção Sow - Alumar|/Producao/ItensEstoque/RegistroProducaoSowAlumar/RegistroProducaoSowAlumar.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group","SHPF|1|Transferências"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/currentlevel","SHPF|1|Transferências|7"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/order","SHPF|1|Transferências|5"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Transferências|Altera Status Interface"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Transferências|Altera Status Interface|/Producao/ItensEstoque/AlterarStatusInterface/AlterarStatusInterface.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Transferências|Transferência Individual de Itens para o EBS"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Transferências|Transferência Individual de Itens para o EBS|/Producao/ItensEstoque/TransferenciaIndividual/TransferenciaIndividual.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Transferências|Transferência em Lote de Itens para o EBS"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Transferências|Transferência em Lote de Itens para o EBS|/Producao/ItensEstoque/TransferenciaEmLote/TransferenciaEmLote.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group","SHPF|1|Balanças"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/currentlevel","SHPF|1|Balanças|9"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/order","SHPF|1|Balanças|6"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Balanças|Consulta de Aferição"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Balanças|Consulta de Aferição|/Producao/Consulta/Balanca/ConsultaAfericao.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Balanças|Cadastro de Balança"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Balanças|Cadastro de Balança|/Producao/Balanca/CadastrodeBalancas.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group","SHPF|1|Kanban"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/currentlevel","SHPF|1|Kanban|10"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/order","SHPF|1|Kanban|6"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Kanban|Tipo"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Kanban|Tipo|/Producao/Kanban/TipoKanban/CadastroDeTiposKanban.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Kanban|Recurso"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Kanban|Recurso|/Producao/Kanban/Recurso/CadastroDeRecursos.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Kanban|Produto"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Kanban|Produto|/Producao/Kanban/ProdutoKanban/CadastroProdutosKanban.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group","SHPF|1|Relatórios"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/currentlevel","SHPF|1|Relatórios|11"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/order","SHPF|1|Relatórios|6"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Relatórios|Itens de Estoque por Família de Produto"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Relatórios|Itens de Estoque por Família de Produto|/Producao/Relatorios/ItensEstoquePorFamilia/ItensEstoquePorFamilia.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Relatórios|Itens de Estoque por Cliente / Produto"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Relatórios|Itens de Estoque por Cliente / Produto|/Producao/Relatorios/ItensEstoquePorClienteProduto/ItensEstoquePorClienteProduto.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Relatórios|Análises por Lote / Corrida"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url",  "SHPF|1|Relatórios|Análises por Lote / Corrida|/Producao/Relatorios/AnalisesPorLoteCorrida/AnalisesPorLoteCorrida.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service",  "SHPF|1|Relatórios|Estoque - Resumo por Ordem de Produção"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url",  "SHPF|1|Relatórios|Estoque - Resumo por Ordem de Produção|/Producao/Relatorios/ResumoPorOrdemDeProducao/ResumoPorOrdemDeProducao.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service",  "SHPF|1|Relatórios|Estoque de Venda - Resumo por Localização"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url",  "SHPF|1|Relatórios|Estoque de Venda - Resumo por Localização|/Producao/Relatorios/ResumoPorLocalizacao/ResumoPorLocalizacao.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group",  "SHPF|1|Programação de Produção"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/currentlevel",  "SHPF|1|Programação de Produção|5"),   
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/order","SHPF|1|Programação de Produção|4"),  
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Programação de Produção|Manutenção por Número de Produção"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Programação de Produção|Manutenção por Número de Produção|/Producao/ManutencaoProducao/ManutencaoProducao.aspx"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Programação de Produção|Sem planejamento EBS"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Programação de Produção|Sem planejamento EBS|/Producao/ProgramacaoSemPlanejamento/PesquisaProducaoSemPlanejamento.aspx"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Programação de Produção|Com Planejamento EBS"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Programação de Produção|Com Planejamento EBS|/Producao/ProgramacaoComPlanejamento/PesquisaProducaoComPlanejamento.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Inventário|Manutenção"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Inventário|Manutenção|/Producao/ItensEstoque/Inventario/Manutencao/Inventario.aspx"),  
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Troca da Ordem de Produção"),  
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Troca da Ordem de Produção|/Producao/ItensEstoque/TrocaOrdemProducao/TrocaOrdemProducao.aspx"),   
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Sucateamento"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Sucateamento|/Producao/ItensEstoque/Sucateamento/Sucateamento.aspx"),  
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Retirada do Estoque de Venda"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Retirada do Estoque de Venda|/Producao/ItensEstoque/RetiradaEBS/RetiradaEBS.aspx"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Reimpressão de Etiquetas"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Reimpressão de Etiquetas|/Producao/ItensEstoque/ReimpressaoEtiqueta/ReimpressaoEtiqueta.aspx"),  
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Reclassificação"),  
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Reclassificação|/Producao/ItensEstoque/Reclassificacao/Reclassificacao.aspx"),  
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Mudança de Localização"),  
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Mudança de Localização|/Producao/ItensEstoque/Realocacao/Realocacao.aspx"),  
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Alterar Status Item"),  
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Alterar Status Item|/Producao/ItensEstoque/AlterarStatusItem/AlterarStatusItem.aspx"),  
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Item Substituto"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Item Substituto|/Producao/ItensEstoque/ItemSubstituto/ItemSubstituto.aspx"),   
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|1|Itens de Estoque|Apontamento de Ingredientes"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|1|Itens de Estoque|Apontamento de Ingredientes|/Producao/ItensEstoque/ApontamentoIngredientes/ApontamentoIngredientes.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile", "SHPF|2"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group","SHPF|2|Apoio"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/currentlevel","SHPF|2|Apoio|1"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/parentlevel","SHPF|2|Apoio|0"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/order","SHPF|2|Apoio|1"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|2|Apoio|Manutenção de Warehouse x Usuários"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|2|Apoio|Manutenção de Warehouse x Usuários|/Apoio/CadastroWarehouseUsuarios.aspx"),   
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|2|Apoio|Parâmetros do Sistema"),   
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|2|Apoio|Parâmetros do Sistema|/Apoio/ParametrosDoSistema.aspx"),               
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|2|Apoio|Parâmetros do Sistema - Romaneio"), 
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|2|Apoio|Parâmetros do Sistema - Romaneio|/Geral/ParametrosRomaneio/ParametrosRomaneio.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service","SHPF|2|Apoio|Reiniciar Sessão"),
                new Claim("http://schemas.alcoa.com/claims/app/profile/group/service/url","SHPF|2|Apoio|Reiniciar Sessão|/Geral/Sessao/ReiniciarSessao.aspx"),
                new Claim("http://schemas.alcoa.com/claims/app/profile","SHPF|3"),
                new Claim("http://schemas.alcoa.com/claims/app/profile","SHPF|4"),
            };

            UserClaims = new ClaimsIdentity(claims, "Kerberos").Claims;
        }

        [TestMethod]
        public void GetApplicationMenu()
        {
            var menu = new MenuModel(UserClaims, string.Empty).GetApplicationMenu();

            Assert.IsNotNull(menu);
            Assert.IsTrue(menu.Count > 0);
        }

        [TestMethod]
        public void GetDynamicApplicationMenu()
        {
            var appMenu = new MenuModel(UserClaims, string.Empty);
            var menu = appMenu.CastToDynamicMenuList(appMenu.GetApplicationMenu());

            Assert.IsNotNull(menu);
            Assert.IsTrue(menu.Count > 0);
        }

        [TestMethod]
        public void GetMenuFindingByText()
        {
            var menu = new MenuModel(UserClaims, string.Empty);
            var appMenus = menu.GetApplicationMenu();
            var menuFound = menu.FindMenu("Producao", appMenus);

            Assert.IsNotNull(menuFound);
        }
    }
}