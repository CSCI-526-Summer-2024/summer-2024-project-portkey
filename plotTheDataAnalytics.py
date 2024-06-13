import requests
import matplotlib.pyplot as plt
import numpy as np
from collections import defaultdict


# fetch data from Firebase
def fetch_data_from_firebase(url):
    response = requests.get(url)
    if response.status_code == 200:
        return response.json()
    else:
        print("Failed to fetch data")
        return {}

# generate the graph
def  generate_the_graph(xlabel, ylabel,title,legend, levels, any):
    x = np.arange(len(levels))  # the label locations
    fig, ax = plt.subplots()
    width = 0.30 
    bars = ax.bar(x, any, width, color='blue')

    # label x and y axis
    ax.set_xlabel(xlabel)
    ax.set_ylabel(ylabel)
    ax.set_title(title)
    ax.set_xticks(x)
    ax.set_xticklabels(levels)
    ax.legend([legend])

    # text on the bars
    for bar in bars:
        height = bar.get_height()
        ax.annotate('{}'.format(round(height, 2)),
                    xy=(bar.get_x() + bar.get_width() / 2, height),
                    xytext=(0, 4),  
                    textcoords="offset points",
                    ha='center', va='bottom')

    fig.tight_layout()
    plt.show()   

# process all data to get level popularity
def process_all_data(data):
    level_counts = defaultdict(int)
    death_counts = defaultdict(list)
    prop_usage = defaultdict(list)
    reason_counts = defaultdict(int)
    level_scores = defaultdict(list)

    for player in data.values():
        level = player['level']
        level_counts[level] += 1
        if player['deathDueToControlsFlip']:
            death_counts[player['level']].append(1)
        else:
            death_counts[player['level']].append(0)
        prop_usage[player['level']].append(player['totalSwitchingPropCollected'])

        reason = player['reasonforFinshingLevel1']
        reason_counts[reason] += 1
        level_scores[player['level']].append(player['score'])
        
    levels = sorted(level_counts.keys())
    counts = [level_counts[level] for level in levels]
    average_deaths = [np.mean(death_counts[level]) for level in levels]
    average_usages = [np.mean(prop_usage[level]) for level in levels]
    reasons = sorted(reason_counts.keys())
    reasons_counts = [reason_counts[reason] for reason in reasons]
    average_scores = [np.mean(level_scores[level]) for level in levels]

    return levels, counts, average_deaths, average_usages, reasons, reasons_counts, average_scores

# firebase db url
all_analytics_url = 'https://portkey-2a1ae-default-rtdb.firebaseio.com/all_analytics.json'

# plots each of the data
all_data = fetch_data_from_firebase(all_analytics_url)
if all_data:
    levels, counts, average_deaths, average_usages, reasons, reasons_counts, average_scores = process_all_data(all_data)
    #plotting metric 1
    generate_the_graph("Levels", "Average Scores", "Average Scores by Level", "Average Score", levels, average_scores)  # 2
    # plotting metric 2
    generate_the_graph("Reason for Finishing Level 1", "Total Number of Games Played", "First Level Completion Reasons", "Total Games", reasons, reasons_counts)
    # plotting metric 3
    generate_the_graph("Levels", "Average Control-Flipping Prop Collection", "Average Control-Flipping Prop Collection by Level'", "Average Collection", levels, average_usages)  #2
    # plotting metric 4
    generate_the_graph("Levels", "Average Deaths After Control Flip", "Average Deaths After Control Flip by Level'", "Average Deaths", levels, average_deaths)  #2
    # plotting metric 5
    generate_the_graph("Levels", "otal Number of Plays", "Level Popularity", "Total Plays", levels, counts) 
else:
    print("No data to display for average scores per level")

