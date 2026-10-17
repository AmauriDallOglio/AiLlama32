# AiLlama32

## O que e este projeto

Este projeto demonstra uma API .NET preparada para integracao com Ollama.

## Funcionalidade

A API possui rotas de inserir, alterar, excluir e pesquisar sessoes personalizadas. Cada sessao guarda modelo, instrucao de sistema, temperatura, tags e historico, podendo ser usada em uma rota de pergunta para recuperar informacoes de acordo com o contexto configurado.

As sessoes sao gravadas em arquivo JSON configurado por `ConfiguracaoOllama:CaminhoArquivoSessoes`, permitindo consultar novamente as sessoes criadas mesmo depois de reiniciar a API.

Tambem possui a rota `POST /api/SessoesOllama/AssuntoGeral`, que envia uma pergunta diretamente para o modelo configurado como `llama3.2:latest`, sem usar sessao personalizada e sem resposta simulada. Para esta rota funcionar, o Ollama deve estar ativo e o modelo `llama3.2:latest` deve estar baixado no servidor configurado.

Para consultar as mensagens gravadas quando `incluirHistorico` for `true`, use `GET /api/SessoesOllama/{id}/historico`. Para limpar o historico em memoria da sessao, use `DELETE /api/SessoesOllama/{id}/historico` ou `DELETE /api/SessoesOllama/{id}/deletar-historico`.

## Objetivo

O objetivo e mostrar como organizar sessoes personalizadas para IA local, permitindo reaproveitar contexto, instrucoes e historico em chamadas futuras. O exemplo funciona com resposta simulada e pode ser configurado para usar Ollama real.

## Onde pode ser usado

Pode ser usado em assistentes internos, suporte tecnico, busca em documentacao, automacoes com IA local, prototipos de chat e APIs hospedadas no Azure que se conectam a um servidor Ollama configurado.
