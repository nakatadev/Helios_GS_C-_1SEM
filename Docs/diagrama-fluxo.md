# Diagrama de fluxo - HELIOS C#

```mermaid
flowchart TD
    A[Iniciar Console] --> B[Criar PainelSolar, Robo e sensores]
    B --> C[Coletar leituras por ISensor]
    C --> D{Sensor online e leitura valida?}
    D -- Nao --> E[Registrar alerta tecnico]
    D -- Sim --> F[Salvar Leitura com DateTime UTC]
    F --> G{Poeira >= 40%?}
    G -- Sim --> H[Gerar alerta SUJEIRA_DETECTADA]
    H --> I[Criar TarefaManutencao]
    I --> J[Emitir Comando LIMPAR]
    J --> K[Robo executa limpeza]
    K --> L[Resolver alerta e concluir tarefa]
    G -- Nao --> M{Corrente < 3.0 A?}
    M -- Sim --> N[Gerar alerta BAIXA_GERACAO]
    M -- Nao --> O[Manter operacao normal]
    E --> P[Resumo final]
    L --> P
    N --> P
    O --> P
```

## Leitura do fluxo

O fluxo representa a regra central do dominio C#: receber leituras simuladas, validar excecoes, registrar historico com datas, decidir sobre alertas e comandar o rover HELIOS quando a poeira ultrapassa o limite.
