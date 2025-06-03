import os
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

#REQUIRES TASK 8 TO HAVE BEEN RUN, AND THE FILES NEED TO BE DIRECTLY IN RAD_PROJECT FOLDER, NOT SUB

with open("true_s.txt", "r") as f:
    true_S = int(f.read().strip())

chaining_runtime = None
if os.path.exists("chaining_runtime.txt"):
    with open("chaining_runtime.txt", "r") as f:
        chaining_runtime = int(f.read().strip())

m_values = [1 << 8, 1 << 14, 1 << 20]

average_runtimes = []
mse_values = []
mean_estimates = []
theoretical_vars = []

fig_sorted, ax_sorted = plt.subplots(figsize=(10, 5))
ax_sorted.axhline(y=true_S, color='black', linestyle='--', label='True S')

fig_medians, ax_medians = plt.subplots(figsize=(10, 5))
ax_medians.axhline(y=true_S, color='black', linestyle='--', label='True S')

for m in m_values:
    print(f"\n--- Results for m = {m} ---")
    t = int(np.log2(m))

    df_sorted = pd.read_csv(f"sorted_estimates_m{m}.csv")
    df_medians = pd.read_csv(f"medians_m{m}.csv")
    df_runtimes = pd.read_csv(f"runtimes_m{m}.csv")

    mean_estimate = df_sorted["estimate"].mean()
    empirical_mse = ((df_sorted["estimate"] - true_S) ** 2).mean()
    theoretical_variance = (2 * true_S**2) / m

    avg_runtime = df_runtimes["runtime_ms"].mean()
    average_runtimes.append(avg_runtime)
    mse_values.append(empirical_mse)
    mean_estimates.append(mean_estimate)
    theoretical_vars.append(theoretical_variance)

    print(f"Mean of estimates (E[X]): {mean_estimate:.2f}")
    print(f"Empirical MSE: {empirical_mse:.2f}")
    print(f"Theoretical Var[X] ≈ 2S²/m: {theoretical_variance:.2f}")
    print(f"Average runtime: {avg_runtime:.2f} ms")

    ax_sorted.plot(
        df_sorted["experiment_index"],
        df_sorted["estimate"],
        marker='o',
        linestyle='',
        markersize=3,
        label=f"m={m}"
    )

    ax_medians.plot(
        df_medians["group_index"],
        df_medians["median"],
        marker='o',
        linestyle='',
        markersize=6,
        label=f"m={m}"
    )

ax_sorted.set_title("Sorted Count-Sketch Estimates vs True S")
ax_sorted.set_xlabel("Experiment Index")
ax_sorted.set_ylabel("Estimated S")
ax_sorted.legend()
ax_sorted.grid(True)
fig_sorted.tight_layout()
fig_sorted.savefig("combined_sorted_estimates.png")

ax_medians.set_title("Group Medians of Count-Sketch Estimates")
ax_medians.set_xlabel("Group Index")
ax_medians.set_ylabel("Median Estimate")
ax_medians.legend()
ax_medians.grid(True)
fig_medians.tight_layout()
fig_medians.savefig("combined_medians.png")

plt.figure(figsize=(10, 4))
plt.plot([f"m={m}" for m in m_values], average_runtimes, marker='o', label="Count-Sketch Avg Runtime")
if chaining_runtime:
    plt.axhline(y=chaining_runtime, color='red', linestyle='--', label='Chaining Runtime')
plt.title("Average Runtime per Experiment vs m")
plt.ylabel("Milliseconds")
plt.xlabel("Sketch Size (m)")
plt.legend()
plt.grid(True)
plt.tight_layout()
plt.savefig("average_runtime_comparison.png")

plt.figure(figsize=(10, 4))
plt.plot([f"m={m}" for m in m_values], mse_values, marker='o', label="Empirical MSE")
plt.plot([f"m={m}" for m in m_values], theoretical_vars, marker='x', linestyle='--', label="Theoretical Var[X]")
plt.title("MSE vs m")
plt.ylabel("Squared Error")
plt.xlabel("Sketch Size (m)")
plt.legend()
plt.grid(True)
plt.tight_layout()
plt.savefig("mse_vs_m.png")

plt.show()
