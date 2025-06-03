import pandas as pd
import matplotlib.pyplot as plt

true_S = None
mse = None

with open("metadata.txt", "r") as f:
    for line in f:
        if line.startswith("True S:"):
            true_S = int(line.split(":")[1].strip())
        elif "Mean Squared Error" in line:
            mse = int(line.split(":")[1].strip())
df_sorted = pd.read_csv("sorted_estimates.csv")
df_medians = pd.read_csv("medians.csv")

mean_estimate = df_sorted["estimate"].mean()

empirical_mse = ((df_sorted["estimate"] - true_S) ** 2).mean()

m = 2 ** 14
theoretical_variance = (2 * true_S**2) / m

# Output results
print(f"Mean of estimates (E[X]): {mean_estimate:.2f}")
print(f"True S: {true_S:.2f}")
print(f"Empirical MSE: {empirical_mse:.2f}")
print(f"Theoretical Var[X] ≈ 2S²/m: {theoretical_variance:.2f}")

plt.figure(figsize=(10, 5))
plt.plot(df_sorted['experiment_index'], df_sorted['estimate'], marker='o', label='Sorted Estimates')
plt.axhline(y=true_S, color='red', linestyle='--', label='True S')
plt.title("Count-Sketch Estimates vs True S")
plt.xlabel("Experiment Index")
plt.ylabel("Estimated S")
plt.legend()
plt.grid(True)
plt.tight_layout()
plt.show()

plt.figure(figsize=(8, 4))
plt.plot(df_medians['group_index'], df_medians['median'], marker='o', label='Group Medians')
plt.axhline(y=true_S, color='red', linestyle='--', label='True S')
plt.title("Group Medians of Count-Sketch Estimates")
plt.xlabel("Group Index")
plt.ylabel("Median Estimate")
plt.legend()
plt.grid(True)
plt.tight_layout()
plt.show()