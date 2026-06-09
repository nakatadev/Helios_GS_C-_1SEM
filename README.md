# HELIOS - Global Solution 2026.1 - C#/.NET

Projeto Console em .NET 8 para a disciplina de C#, alinhado ao tema de industria espacial da Global Solution.

## Motivacao

O HELIOS representa o cerebro de regras de um rover autonomo que monitora paineis solares de uma base lunar. Como a poeira lunar reduz a geracao de energia e o controle remoto Terra-Lua tem atraso, o sistema precisa decidir localmente quando um painel esta sujo, gerar alerta, criar tarefa de manutencao e emitir comando de limpeza.

Nesta entrega de C#, o foco e a modelagem de dominio, POO, abstracao, interfaces, regras de negocio, datas e tratamento de excecoes. Itens opcionais do enunciado, como banco de dados ou manipulacao de arquivos como persistencia principal, nao foram implementados.

## Como executar

Requisitos:

- .NET SDK 8 instalado

Comandos:

```bash
cd Helios
dotnet build
dotnet run
```

A evidencia de uma execucao foi salva em:

```text
Docs/evidencias-execucao.txt
```
Segue as imagens abaixo sobre a execução:

<img width="895" height="630" alt="image" src="https://github.com/user-attachments/assets/7c2839c8-d632-47d8-bacf-d92209b7c36e" />
<img width="894" height="626" alt="image" src="https://github.com/user-attachments/assets/894673e5-6045-4800-85a9-016344370b80" />

## Como integra com a GS

O projeto usa o mesmo contrato de dados definido no briefing para leituras, alertas e comandos:

```json
{"sensorId":"PANEL-A-CURRENT","tipo":"corrente","valor":4.2,"unidade":"A","ativoId":"PAINEL-A","timestamp":"2026-06-01T14:03:00.0000000Z"}
{"alertaId":"ALT-002","ativoId":"PAINEL-A","severidade":"ALTA","tipo":"SUJEIRA_DETECTADA","mensagem":"Cobertura de poeira em 47.5%","timestamp":"2026-06-01T14:08:00.0000000Z","resolvido":false}
{"comandoId":"CMD-140801","robotId":"HELIOS-01","acao":"LIMPAR","alvoAtivoId":"PAINEL-A","timestamp":"2026-06-01T14:08:01.0000000Z"}
```

Na arquitetura geral, este projeto e o dominio C# consumido ou replicado pelos servicos Java/SOA e pelo app mobile:

```text
Visao/Python -> evento de sujeira -> Servicos Java/SOA -> Dominio C# -> comando LIMPAR -> App Mobile
```

## Fluxo da simulacao

1. O Console cria um painel solar, o rover HELIOS e quatro sensores.
2. Cada ciclo coleta leituras de corrente, poeira, temperatura e luz.
3. As leituras sao armazenadas em repositorios em memoria com `DateTime` UTC.
4. Se a corrente cai abaixo do limite, o sistema gera alerta de baixa geracao.
5. Se a poeira passa de 40%, o sistema gera alerta de sujeira, cria tarefa, emite comando `LIMPAR`, simula a limpeza e resolve o alerta.
6. Ao final, o sistema simula um sensor offline para demonstrar excecao controlada.

Diagrama detalhado: [Docs/diagrama-fluxo.md](Docs/diagrama-fluxo.md)

## Mapa dos requisitos obrigatorios

| Requisito | Onde aparece |
|---|---|
| Classes publicas, estaticas e privadas | `Program` static, entidades publicas, campos privados em sensores/repositorios |
| Entidades do desafio | `PainelSolar`, `Sensor`, `Leitura`, `Alerta`, `Robo`, `Comando`, `TarefaManutencao` |
| Heranca | `Ativo` -> `PainelSolar`; `Sensor` -> sensores especificos |
| Polimorfismo | Lista `List<ISensor>` processa sensores diferentes pelo mesmo contrato |
| Classe abstrata | `Domain/Sensors/Sensor.cs` e `Domain/Assets/Ativo.cs` |
| Interfaces | `ISensor`, `IRepositorio<T>`, `INotificador` |
| Injecao de dependencia | `HeliosRuleEngine` recebe repositorios e notificador pelo construtor |
| Metodos e funcoes | Regras separadas em `ProcessarCiclo`, `AvaliarLeitura`, `RegistrarAlerta` |
| DateTime | Leituras, alertas, comandos e tarefas usam timestamp UTC |
| Excecoes | `SensorOfflineException` e `LeituraInvalidaException` |
| Structs | `Coordenada` e `LeituraValor` |
| Partial | `Robo.cs` e `Robo.Operacoes.cs` |
| Organizacao | Pastas por responsabilidade: `Domain`, `Contracts`, `Services`, `Repositories`, `Exceptions`, `Docs` |
| Evidencia de execucao | `Docs/evidencias-execucao.txt` |

## Estrutura de pastas

```text
Helios/
  Contracts/       Interfaces do dominio
  Domain/          Entidades, structs, enums e regras naturais do problema
  Exceptions/      Excecoes especificas
  Repositories/    Repositorio generico em memoria
  Services/        Motor de regras e notificacao no console
  Docs/            Diagrama e evidencias
  Program.cs       Entrada da simulacao
```

## Integrantes

```
Rodrigo Nakata - RM 556417
Gabriel Henrique Padula - RM 554907
Arthur Abonizio - RM 555506
```
